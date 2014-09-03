using System;	
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using Sharp.Data.Util;
using System.Collections.Generic;
using System.Globalization;
using Sharp.Data.Schema;
using Sharp.Util;

namespace Sharp.Data.Databases.SqlServer {
    public class SqlDialect : Dialect {

        public static string ExtendedPropertyNameForComments = "MS_Description";

		public override string ParameterPrefix {
            get {
                return "@";
            }
        }

        public override DbType GetDbType(string sqlType, int dataPrecision) {
            throw new NotImplementedException();
        }

        public override string[] GetCreateTableSqls(Table table) {

            var sqls = new List<string>();
            var primaryKeyColumns = new List<string>();
            
            //create table
            var sb = new StringBuilder();
            sb.Append("create table ").Append(table.Name).AppendLine(" ( ");

            int size = table.Columns.Count;
            for (int i = 0; i < size; i++) {
                sb.Append(GetColumnToSqlWhenCreate(table.Columns[i]));
                if (i != size - 1) {
                    sb.AppendLine(",");
                }
                if (table.Columns[i].IsPrimaryKey) {
                    primaryKeyColumns.Add(table.Columns[i].ColumnName);
                }
            }
            sb.AppendLine(")");

            sqls.Add(sb.ToString());
            
            //primary key
            if (primaryKeyColumns.Count > 0) {
                sqls.Add(GetPrimaryKeySql(table.Name, String.Format("pk_{0}", table.Name), primaryKeyColumns.ToArray()));
            }
            return sqls.ToArray();
        }

        public override string GetColumnToSqlWhenCreate(Column col) {
            string colType = GetDbTypeString(col.Type, col.Size);
            string colNullable = col.IsNullable ? WordNull : WordNotNull;
            string colAutoIncrement = col.IsAutoIncrement ? "identity(1,1)" : "";

            string colDefault = (col.DefaultValue != null) ?
                String.Format("default ({0})", GetColumnValueToSql(col.DefaultValue)) : "";

            //name type default nullable autoIncrement
            return String.Format("{0} {1} {2} {3} {4}", col.ColumnName, colType, colNullable, colDefault, colAutoIncrement);
        }

        public override string[] GetDropTableSqls(string tableName) {
            string sql = String.Format("drop table {0}", tableName);
            return new[] { sql };
        }

        public override string[] GetDropColumnSql(string table, string columnName) {
        	string findDefaultConstraint = String.Format(
				@"select d.name
					from sys.tables t
						join
						sys.default_constraints d
							on d.parent_object_id = t.object_id
						join
						sys.columns c
							on c.object_id = t.object_id
							and c.column_id = d.parent_column_id
					where t.name = '{0}'
					and c.name = '{1}'", table, columnName);
			string dropDefaultConstrant = String.Format("ALTER TABLE {0} DROP CONSTRAINT [{{0}}]", table);
        	string dropColumn = String.Format("ALTER TABLE {0} DROP COLUMN {1}", table, columnName);
			return new[] { findDefaultConstraint, dropDefaultConstrant, dropColumn };
        }

        public override string GetForeignKeySql(string fkName, string table, string column, string referencingTable, string referencingColumn, OnDelete onDelete) {
            string onDeleteSql;
            switch (onDelete) {
                case OnDelete.Cascade: onDeleteSql = "on delete cascade"; break;
                case OnDelete.SetNull: onDeleteSql = "on delete set null"; break;
                case OnDelete.NoAction: onDeleteSql = "on delete no action"; break;
                default: onDeleteSql = ""; break;
            }

            return String.Format("alter table {0} add constraint {1} foreign key ({2}) references {3}({4}) {5}",
                    table,
                    fkName,
                    column,
                    referencingTable,
                    referencingColumn,
                    onDeleteSql);
        }

    	public override string GetDropIndexSql(string indexName, string table) {
			return String.Format("drop index {0} on {1}", indexName, table);
		}

    	public override string GetUniqueKeySql(string ukName, string table, params string[] columnNames) {
            return String.Format("alter table {0} add constraint {1} unique ({2})",
                                  table,
                                  ukName,
                                  StringHelper.Implode(columnNames, ","));
        }

        public override string GetDropUniqueKeySql(string uniqueKeyName, string tableName) {
            return String.Format("alter table {0} drop constraint {1}", tableName, uniqueKeyName);
        }

        public override string GetInsertReturningColumnSql(string table, string[] columns, object[] values, string returningColumnName, string returningParameterName) {
            string sql = GetInsertSql(table, columns, values);
            sql = sql.Replace(") values (", ") output Inserted." + returningColumnName + " into @tempTable values (");

            return String.Format("declare @tempTable TABLE (id int); {0}; select @{1} = id from @tempTable",
                                  sql, returningParameterName);
        }

    	public override string WrapSelectSqlWithPagination(string sql, int skipRows, int numberOfRows) {
            var regex = new Regex("SELECT", RegexOptions.IgnoreCase);
    		sql = regex.Replace(sql, "SELECT TOP 2147483647 ", 1);
    		string innerSql =
				@"select * into #TempTable from (
							select * ,ROW_NUMBER() over(order by aaa) AS rownum from (
								select 'aaa' as aaa, * from  (
									{0}
								)as t1
							)as t2
						) as t3
					where rownum between {1} and {2}
					alter table #TempTable drop column aaa
					alter table #TempTable drop column rownum
					select * from #TempTable
					drop table #TempTable
				";
			return String.Format(innerSql, sql, skipRows+1, skipRows + numberOfRows);
    	}

    	protected override string GetDbTypeString(DbType type, int precision) {
            switch (type) {
                case DbType.AnsiStringFixedLength:
                    if (precision <= 0) return "CHAR(255)";
                    if (precision.Between(1, 255)) return String.Format("CHAR({0})", precision);
                    if (precision.Between(256, 65535)) return "TEXT";
                    if (precision.Between(65536, 16777215)) return "MEDIUMTEXT";
                    break;
                case DbType.AnsiString:
                    if (precision <= 0) return "VARCHAR(255)";
                    if (precision.Between(1, 255)) return String.Format("VARCHAR({0})", precision);
                    if (precision.Between(256, 65535)) return "TEXT";
                    if (precision.Between(65536, 16777215)) return "MEDIUMTEXT";
                    break;
                case DbType.Binary: return "BINARY";
                case DbType.Boolean: return "BIT";
                case DbType.Byte: return "TINYINT UNSIGNED";
                case DbType.Currency: return "MONEY";
                case DbType.Date: return "DATETIME";
                case DbType.DateTime: return "DATETIME";
                case DbType.Decimal:
                    if (precision <= 0) return "NUMERIC(19,5)";
                    return String.Format("NUMERIC(19,{0})", precision);
                case DbType.Double: return "FLOAT";
                case DbType.Guid: return "VARCHAR(40)";
                case DbType.Int16: return "SMALLINT";
                case DbType.Int32: return "INTEGER";
                case DbType.Int64: return "BIGINT";
                case DbType.Single: return "FLOAT";
                case DbType.StringFixedLength:
                    if (precision <= 0) return "CHAR(255)";
                    if (precision.Between(1, 255)) return String.Format("CHAR({0})", precision);
                    if (precision.Between(256, 65535)) return "TEXT";
                    if (precision.Between(65536, 16777215)) return "MEDIUMTEXT";
                    break;
                case DbType.String:
                    if (precision <= 0) return "VARCHAR(255)";
                    if (precision.Between(1, 255)) return String.Format("VARCHAR({0})", precision);
                    if (precision.Between(256, 65535)) return "TEXT";
                    if (precision.Between(65536, 16777215)) return "MEDIUMTEXT";
                    break;
                case DbType.Time: return "TIME";
            }
            throw new DataTypeNotAvailableException(String.Format("The type {0} is no available for sqlserver", type.ToString()));
        }

        public override string GetColumnValueToSql(object value) {
            if (value is bool) {
                return ((bool)value) ? "1" : "0";
            }

            if ((value is Int16) || (value is Int32) || (value is Int64) || (value is double) || (value is float) || (value is decimal)) {
                return Convert.ToString(value, CultureInfo.InvariantCulture);
            }

            if (value is DateTime) {
                var dt = (DateTime)value;
                return String.Format("'{0}'", dt.ToString("s"));
            }

            return String.Format("'{0}'", value);
        }

		public override string GetTableExistsSql(string tableName) {
			return String.Format("SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'", tableName);
		}

        public override string GetAddCommentToColumnSql(string tableName, string columnName, string comment) {
            return String.Format(@"
                declare @CurrentUser sysname; 
                select @CurrentUser = user_name(); 
                execute sp_addextendedproperty '{0}', '{1}', 'user', @CurrentUser, 'table', '{2}', 'column', '{3}'", 
                ExtendedPropertyNameForComments, comment, tableName, columnName);
        }

        public override string GetAddCommentToTableSql(string tableName, string comment) {
            return String.Format(@"
                declare @CurrentUser sysname; 
                select @CurrentUser = user_name(); 
                execute sp_addextendedproperty '{0}', '{1}', 'user', @CurrentUser, 'table', '{2}'", 
                ExtendedPropertyNameForComments, comment, tableName);
        }

        public override string GetRemoveCommentFromColumnSql(string tableName, string columnName) {
            return String.Format(@"
                declare @CurrentUser sysname; 
                select @CurrentUser = user_name(); 
                execute sp_dropextendedproperty '{0}', 'user', @CurrentUser, 'table', '{1}', 'column', '{2}' ", 
                ExtendedPropertyNameForComments, tableName, columnName);
        }

        public override string GetRemoveCommentFromTableSql(string tableName) {
            return String.Format(@"
                declare @CurrentUser sysname; 
                select @CurrentUser = user_name(); 
                execute sp_dropextendedproperty '{0}', 'user', @CurrentUser, 'table', '{1}'", 
                ExtendedPropertyNameForComments, tableName);
        }

        public override string GetRenameTableSql(string tableName, string newTableName) {
            return String.Format("execute sp_rename '{0}', '{1}'", tableName, newTableName);
        }

        public override string GetRenameColumnSql(string tableName, string columnName, string newColumnName) {
            return String.Format("execute sp_rename '{0}.{1}', '{2}', 'column'", tableName, columnName, newColumnName);
        }

        public override string GetModifyColumnSql(string tableName, string columnName, Column columnDefinition) {
            return String.Format("alter table {0} alter column {1}", tableName, GetColumnToSqlWhenCreate(columnDefinition));
        }
    }
}
