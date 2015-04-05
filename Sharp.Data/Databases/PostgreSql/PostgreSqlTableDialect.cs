using System;
using System.Collections.Generic;
using System.Text;
using Sharp.Data.Schema;

namespace Sharp.Data.Databases.PostgreSql {
    public class PostgreSqlTableDialect {
        public static string SequencePrefix = "SEQ_";
        public static string PrimaryKeyPrefix = "PK_";

        public string[] GetCreateTableSqls(Table table, Func<Column, string> getColumnToSqlWhenCreate, Func<string, string, string[], string> getPrimaryKeySql) {
            var sqls = new List<string>();
            var primaryKeyColumns = new List<string>();
            var tableName = table.Name;
            Column autoIncrementColumn = null;

            //create table
            var sb = new StringBuilder();
            sb.Append("CREATE TABLE ").Append(table.Name).AppendLine(" (");

            var size = table.Columns.Count;
            for (var i = 0; i < size; i++) {
                var column = table.Columns[i];

                sb.Append(getColumnToSqlWhenCreate(column));
                if (i != size - 1) {
                    sb.AppendLine(",");
                }

                if (column.IsAutoIncrement) {
                    autoIncrementColumn = column;
                }
                if (column.IsPrimaryKey) {
                    primaryKeyColumns.Add(column.ColumnName);
                }
            }
            sb.AppendLine(")");
            sqls.Add(sb.ToString());

            SetAutoIncrementColumn(autoIncrementColumn, tableName, sqls);
            SetPrimaryKey(primaryKeyColumns, sqls, tableName, getPrimaryKeySql);

            return sqls.ToArray();
        }

        public string[] GetDropTableSqls(string tableName) {
            return new[] {
                String.Format("DROP TABLE {0} CASCADE", tableName), 
                String.Format("DROP SEQUENCE IF EXISTS {0}{1} CASCADE", SequencePrefix, tableName)
            };
        }

        private void SetPrimaryKey(List<string> primaryKeyColumns, ICollection<string> sqls, string tableName, Func<string, string, string[], string> getPrimaryKeySql) {
            if (primaryKeyColumns.Count > 0) {
                sqls.Add(getPrimaryKeySql(tableName, String.Format("{0}{1}", PrimaryKeyPrefix, tableName), primaryKeyColumns.ToArray()));
            }
        }

        private static void SetAutoIncrementColumn(Column autoIncrementColumn, string tableName, ICollection<string> sqls) {
            if (autoIncrementColumn == null) {
                return;
            }
            var sequenceName = SequencePrefix + tableName;
            sqls.Add(String.Format("CREATE SEQUENCE {0} INCREMENT 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1", sequenceName));
            sqls.Add(String.Format("ALTER TABLE {0} ALTER COLUMN {1} SET DEFAULT NEXTVAL(\'{2}\'::REGCLASS)", tableName, autoIncrementColumn.ColumnName, sequenceName));
        }

        public string GetRenameTableSql(string tableName, string newTableName) {
            return String.Format("ALTER TABLE {0} RENAME TO {1}", tableName, newTableName);
        }

        public string GetTableExistsSql(string tableName) {
            return String.Format("SELECT COUNT(relname) FROM pg_class WHERE relname = '{0}'", tableName);
        }

        public string GetAddCommentToTableSql(string tableName, string comment) {
            return String.Format("COMMENT ON TABLE {0} IS '{1}'", tableName, comment);
        }

        public string GetRemoveCommentToTableSql(string tableName) {
            return String.Format("COMMENT ON TABLE {0} IS ''", tableName);
        }
    }
}