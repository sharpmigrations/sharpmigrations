using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using Sharp.Data.Schema;
using Sharp.Util;

namespace Sharp.Data {
	public class OracleDialect : Dialect {
		public override string ParameterPrefix {
			get { return ":"; }
		}

		public override string[] GetCreateTableSqls(Table table) {
			var sqls = new List<string>();
			var primaryKeyColumns = new List<string>();
			Column autoIncrement = null;

			//create table
			var sb = new StringBuilder();
			sb.Append("create table ").Append(table.Name).AppendLine(" (");

			int size = table.Columns.Count;
			for (int i = 0; i < size; i++) {
				sb.AppendLine(GetColumnToSqlWhenCreate(table.Columns[i]));
				if (i != size - 1) {
					sb.AppendLine(",");
				}
				if (table.Columns[i].IsAutoIncrement) {
					autoIncrement = table.Columns[i];
				}
				if (table.Columns[i].IsPrimaryKey) {
					primaryKeyColumns.Add(table.Columns[i].ColumnName);
				}
			}
			sb.AppendLine(")");
			sqls.Add(sb.ToString());

			//create sequence and trigger for the autoincrement
			if (autoIncrement != null) {
				//create sequence in case of autoincrement
				sb = new StringBuilder();
				sb.AppendFormat("create sequence SEQ_{0} minvalue 1 maxvalue 999999999999999999999999999 ", table.Name);
				sb.Append("start with 1 increment by 1 cache 20");
				sqls.Add(sb.ToString());

				//create trigger to run the sequence
				sb = new StringBuilder();
				sb.AppendFormat("create or replace trigger \"TR_INC_{0}\" before insert on {0} for each row ", table.Name);
				sb.AppendFormat("when (new.{0} is null) ", autoIncrement.ColumnName);
				sb.AppendFormat("begin select seq_{0}.nextval into :new.{1} from dual; end TR_INC_{0};", table.Name,
				                autoIncrement.ColumnName);
				sqls.Add(sb.ToString());
			}
			//primary key
			if (primaryKeyColumns.Count > 0) {
				sqls.Add(GetPrimaryKeySql(String.Format("pk_{0}", table.Name), table.Name, primaryKeyColumns.ToArray()));
			}
            //comments
            sqls.AddRange(GetColumnCommentsSql(table));
			return sqls.ToArray();
		}

		public override string[] GetDropTableSqls(string tableName) {
			var sqls = new string[2];
			sqls[0] = String.Format("drop table {0} cascade constraints", tableName);
			sqls[1] = String.Format("begin execute immediate 'drop sequence SEQ_{0}'; exception when others then null; end;",
			                        tableName);
			return sqls;
		}

		public override string GetPrimaryKeySql(string pkName, string table, params string[] columnNames) {
			if (columnNames.Length == 0) {
				throw new ArgumentException("No columns specified for primary key");
			}

			var sb = new StringBuilder();
			sb.AppendFormat("alter table {0} add constraint {1} primary key (", table, pkName);
			foreach (string col in columnNames) {
				sb.Append(col).AppendLine(",");
			}
			sb.Remove(sb.Length - 3, 1);
			sb.Append(")");
			return sb.ToString();
		}

		public override string GetForeignKeySql(string fkName, string table, string column, string referencingTable,
		                                        string referencingColumn, OnDelete onDelete) {
			string onDeleteSql;
			switch (onDelete) {
				case OnDelete.Cascade:
					onDeleteSql = "on delete cascade";
					break;
				case OnDelete.SetNull:
					onDeleteSql = "on delete set null";
					break;
				default:
					onDeleteSql = "";
					break;
			}

			return String.Format("alter table {0} add constraint {1} foreign key ({2}) references {3} ({4}) {5}",
			                     table,
			                     fkName,
			                     column,
			                     referencingTable,
			                     referencingColumn,
			                     onDeleteSql);
		}

		public override string GetUniqueKeySql(string ukName, string table, params string[] columnNames) {
			return String.Format("create unique index {0} on {1} ({2})", ukName, table, StringHelper.Implode(columnNames, ","));
		}

		public override string GetDropUniqueKeySql(string uniqueKeyName, string tableName) {
			return "drop index " + uniqueKeyName;
		}

        public override string GetDropIndexSql(string indexName, string table) {
            return String.Format("drop index {0}", indexName);
        }

		public override string GetInsertReturningColumnSql(string table, string[] columns, object[] values,
		                                                   string returningColumnName, string returningParameterName) {
			return String.Format("{0} returning {1} into {2}{3}",
			                     GetInsertSql(table, columns, values),
			                     returningColumnName,
			                     ParameterPrefix,
			                     returningParameterName);
		}

		public override string WrapSelectSqlWithPagination(string sql, int skipRows, int numberOfRows) {
			string innerSql = String.Format("select /* FIRST_ROWS(n) */ a.*, ROWNUM rnum from ({0}) a where ROWNUM <= {1}", sql,
			                                skipRows + numberOfRows);
			return String.Format("select * from ({0}) where rnum > {1}", innerSql, skipRows);
		}

		public override string GetColumnToSqlWhenCreate(Column col) {
			string colType = GetDbTypeString(col.Type, col.Size);
			string colNullable = col.IsNullable ? WordNull : WordNotNull;

			string colDefault = (col.DefaultValue != null)
			                    	? String.Format(" default {0}", GetColumnValueToSql(col.DefaultValue))
			                    	: "";

			//name type default nullable
			return String.Format("{0} {1}{2} {3}", col.ColumnName, colType, colDefault, colNullable);
		}

		public override string GetColumnValueToSql(object value) {
			if (value is bool) {
				return ((bool) value) ? "1" : "0";
			}

			if ((value is Int16) || (value is Int32) || (value is Int64) || (value is double) || (value is float) ||
			    (value is decimal)) {
				return Convert.ToString(value, CultureInfo.InvariantCulture);
			}

			if (value is DateTime) {
				var dt = (DateTime) value;
				return String.Format("to_date('{0}','dd/mm/yyyy hh24:mi:ss')", dt.ToString("d/M/yyyy H:m:s"));
			}

			return String.Format("'{0}'", value.ToString());
		}

		protected override string GetDbTypeString(DbType type, int precision) {
			switch (type) {
				case DbType.AnsiString:
                    if(precision == 0) return "CHAR(255)";
                    if(precision <= 4000) return "VARCHAR2(" + precision + ")";
                    return "CLOB";
				case DbType.Binary:
					return "BLOB";
				case DbType.Boolean:
					return "NUMBER(1)";
				case DbType.Byte:
					return "NUMBER(3)";
				case DbType.Currency:
					return "NUMBER(19,1)";
				case DbType.Date:
					return "DATE";
				case DbType.DateTime:
					return "DATE";
				case DbType.Decimal:
					return "NUMBER(19,5)";
				case DbType.Double:
					return "FLOAT";
				case DbType.Guid:
					return "CHAR(38)";
				case DbType.Int16:
					return "NUMBER(5)";
				case DbType.Int32:
					return "NUMBER(10)";
				case DbType.Int64:
					return "NUMBER(20)";
				case DbType.Single:
					return "FLOAT(24)";
				case DbType.String:
                    if(precision == 0) return "VARCHAR2(255)";
                    if(precision <= 4000) return "VARCHAR2(" + precision + ")";
                    return "NCLOB";
				case DbType.Time:
					return "DATE";
			}

			throw new DataTypeNotAvailableException(String.Format("The type {0} is no available for oracle", type));
		}

		public override DbType GetDbType(string sqlType, int dataPrecision) {
			switch (sqlType) {
				case "varchar2":
				case "varchar":
				case "char":
				case "nchar":
				case "nvarchar2":
				case "rowid":
					return DbType.String;
				case "nclob":
				case "clob":
					return DbType.AnsiString;
				case "number":
					return DbType.Decimal;
				case "float":
					return DbType.Double;
				case "raw":
				case "long raw":
				case "blob":
					return DbType.Binary;
				case "date":
				case "timestamp":
					return DbType.DateTime;
				default:
					return DbType.String;
			}
		}

		public override string GetTableExistsSql(string tableName) {
			return String.Format("select count(table_name) from user_tables where upper(table_name) = upper('{0}')", tableName);
		}

        public override string GetAddCommentToColumnSql(string tableName, string columnName, string comment) {
            return String.Format("COMMENT ON COLUMN {0}.{1} IS '{2}'", tableName, columnName, comment);
        }

        public override string GetAddCommentToTableSql(string tableName, string comment) {
            return String.Format("COMMENT ON TABLE {0} IS '{1}'", tableName, comment);
        }

        public override string GetRemoveCommentFromColumnSql(string tableName, string columnName) {
            return String.Format("COMMENT ON COLUMN {0}.{1} IS ''", tableName, columnName);
        }

        public override string GetRemoveCommentFromTableSql(string tableName) {
            return String.Format("COMMENT ON TABLE {0} IS ''", tableName);
        }

        public override string GetRenameTableSql(string tableName, string newTableName) {
            return String.Format("ALTER TABLE {0} RENAME TO {1}", tableName, newTableName);
        }

        public override string GetRenameColumnSql(string tableName, string columnName, string newColumnName) {
            return String.Format("ALTER TABLE {0} RENAME COLUMN {1} TO {2}", tableName, columnName, newColumnName);
        }


		//public OracleDialect() : base() { }

		//public OracleDialect(Database database) : base(database) { }

		//private const string TABLE_COLUMN_SQL = "SELECT user, a.table_name, a.column_name, a.column_id, a.data_default, " +
		//                                        "       a.nullable, a.data_type, a.char_length, a.data_precision, a.data_scale " +
		//                                        "  FROM user_tab_columns a " +
		//                                        " WHERE lower(a.table_name) = lower(:tableName)";

		//private const string TABLE_KEYS = "SELECT a.constraint_name, a.constraint_type, b.column_name " +
		//                                  "FROM user_constraints a, user_cons_columns b " +
		//                                 "WHERE a.constraint_name = b.constraint_name " +
		//                                  "AND a.constraint_type IN ('R', 'P') " +
		//                                  "and lower(a.table_name) = lower(:tableName)";

		//public override List<Column> GetAllColumns(string table) {
		//    ResultSet t = _database.Query(TABLE_COLUMN_SQL, table);

		//    List<Column> list = new List<Column>();

		//    int rows = t.Count;
		//    t.ForEach(p => list.Add(
		//        new Column(p["column_name"].ToString()) {
		//            Type = GetDbType(p["data_type"].ToString(), Convert.ToInt32(p["data_precision"]))
		//        }
		//    ));

		//    ResultSet tableKeys = _database.Query(TABLE_KEYS, table);

		//    //get PK from database table
		//    var pk = (from p in tableKeys
		//              where p["CONSTRAINT_TYPE"].Equals("P")
		//              select p).FirstOrDefault();

		//    //table has to have a PK so we can identify it
		//    if (pk == null) {

		//        //get column matching pk
		//        var colPk = (from p in list
		//                     where p.ColumnName.Equals(pk["COLUMN_NAME"])
		//                     select p).FirstOrDefault();

		//        colPk.IsPrimaryKey = true;
		//    }

		//    return list;
		//}
	}
}