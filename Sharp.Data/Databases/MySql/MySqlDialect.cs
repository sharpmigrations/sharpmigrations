using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using Sharp.Data.Schema;
using Sharp.Util;

namespace Sharp.Data.Databases.MySql {
    public class MySqlDialect : Dialect {
        
        public override string ParameterPrefix {
            get { return ":"; }
        }

        public override DbType GetDbType(string sqlType, int dataPrecision) {
            throw new NotImplementedException();
        }

        public override string[] GetCreateTableSqls(Table table) {
			List<string> sqls = new List<string>();
			List<string> primaryKeyColumns = new List<string>();

			//create table myTable (id int not null auto_increment, name VARCHAR(255) not null, primary key(id))

			//create table
			StringBuilder sb = new StringBuilder();
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

			//primary keys
			if (primaryKeyColumns.Count > 0) {
				sb.AppendLine(String.Format(", primary key ({0})", StringHelper.Implode(primaryKeyColumns.ToArray(),",")));
			}

			sb.AppendLine(")");

			sqls.Add(sb.ToString());
			
			return sqls.ToArray();
        }

        public override string[] GetDropTableSqls(string tableName) {
        	return new[] {String.Format("drop table {0}",tableName)};
        }

        public override string GetPrimaryKeySql(string pkName, string table, params string[] columnNames) {
			throw new NotImplementedException();
        }

        public override string GetForeignKeySql(string fkName, string table, string column, string referencingTable, string referencingColumn, OnDelete onDelete) {
            throw new NotImplementedException();
        }

    	public override string GetUniqueKeySql(string ukName, string table, params string[] columnNames) {
            throw new NotImplementedException();
        }

        public override string GetDropUniqueKeySql(string uniqueKeyName, string tableName) {
            throw new NotImplementedException();
        }

        public override string GetInsertReturningColumnSql(string table, string[] columns, object[] values, string returningColumnName, string returningParameterName) {
            throw new NotImplementedException();
        }

    	public override string WrapSelectSqlWithPagination(string sql, int skipRows, int numberOfRows) {
    		throw new NotImplementedException();
    	}

    	protected override string GetDbTypeString(DbType type, int precision) {
            switch (type) {
                case DbType.String:
                    if (precision > 0) {
                        return "VARCHAR(" + precision + ")";
                    }
                    return "VARCHAR(255)";
				case DbType.Binary: return "BINARY";
				case DbType.Double: return "DOUBLE";
				case DbType.Boolean: return "BOOLEAN";
				case DbType.Decimal: return "DECIMAL";
				case DbType.Guid: return "VARCHAR(40)";
				case DbType.Int16: return "SMALLINT";
				case DbType.Int32: return "INT";
				case DbType.Single: return "INT";
				case DbType.Int64: return "BIGINT";
				case DbType.Date: return "DATETIME";
            }

            throw new DataTypeNotAvailableException(String.Format("The type {0} is no available for mysql", type.ToString()));
        }

        public override string GetColumnToSqlWhenCreate(Column col) {
            string nullOption = col.IsNullable ? WordNull : WordNotNull;
            string autoIncrement = col.IsAutoIncrement ? " AUTO_INCREMENT" : "";
            string defaultValue = col.DefaultValue != null ? " default "+GetColumnValueToSql(col.DefaultValue): "";

            return String.Format("{0} {1} {2}{3}{4}", col.ColumnName, GetDbTypeString(col.Type, col.Size), nullOption, defaultValue, autoIncrement);
        }

        public override string GetColumnValueToSql(object value) {
            if (value is bool) {
                return ((bool)value) ? "1" : "0";
            }

            if ((value is Int16) || (value is Int32) || (value is Int64) || (value is double) || (value is float) || (value is decimal)) {
                return Convert.ToString(value, CultureInfo.InvariantCulture);
            }

            if (value is DateTime) {
                DateTime dt = (DateTime)value;
                return String.Format("'{0}'", dt.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            return String.Format("'{0}'", value);
        }

    	public override string GetTableExistsSql(string tableName) {
    		return "select count(TABLE_NAME) from INFORMATION_SCHEMA where TABLE_NAME = '"+tableName+"'";
    	}
    }
}
