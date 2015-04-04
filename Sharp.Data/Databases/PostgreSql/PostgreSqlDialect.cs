using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using Sharp.Data.Schema;
using Sharp.Util;

namespace Sharp.Data.Databases.PostgreSql {
    public class PostgreSqlDialect : Dialect {
        public static string SequencePrefix = "SEQ_";
        public static string PrimaryKeyPrefix = "PK_";

        public override string ParameterPrefix {
            get { return ":"; }
        }

        public override DbType GetDbType(string sqlType, int dataPrecision) {
            throw new NotImplementedException();
        }

        public override string[] GetCreateTableSqls(Table table) {
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

                sb.Append(GetColumnToSqlWhenCreate(column));
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
            SetPrimaryKey(primaryKeyColumns, sqls, tableName);

            return sqls.ToArray();
        }

        private void SetPrimaryKey(List<string> primaryKeyColumns, ICollection<string> sqls, string tableName) {
            if (primaryKeyColumns.Count > 0) {
                sqls.Add(GetPrimaryKeySql(tableName, String.Format("{0}{1}", PrimaryKeyPrefix, tableName), primaryKeyColumns.ToArray()));
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

        public override string[] GetDropTableSqls(string tableName) {
            return new[] {
                String.Format("DROP TABLE {0} CASCADE", tableName), 
                String.Format("DROP SEQUENCE IF EXISTS {0}{1} CASCADE", SequencePrefix, tableName)
            };
        }

        public override string GetForeignKeySql(string fkName, string table, string column, string referencingTable, string referencingColumn, OnDelete onDelete) {
            string onDeleteSql;
            switch (onDelete) {
                case OnDelete.Cascade:
                    onDeleteSql = "ON DELETE CASCADE";
                    break;
                case OnDelete.SetNull:
                    onDeleteSql = "ON DELETE SET NULL";
                    break;
                default:
                    onDeleteSql = "";
                    break;
            }

            return String.Format("ALTER TABLE {0} ADD CONSTRAINT {1} FOREIGN KEY ({2}) REFERENCES {3} ({4}) {5}",
                             table,
                             fkName,
                             column,
                             referencingTable,
                             referencingColumn,
                             onDeleteSql);
        }

        public override string GetUniqueKeySql(string ukName, string table, params string[] columnNames) {
            return String.Format("ALTER TABLE {0} ADD CONSTRAINT {1} UNIQUE ({2})", table, ukName, StringHelper.Implode(columnNames, ","));
        }

        public override string GetDropUniqueKeySql(string uniqueKeyName, string tableName) {
            return String.Format("ALTER TABLE {0} DROP CONSTRAINT {1}", tableName, uniqueKeyName);
        }

        public override string GetInsertReturningColumnSql(string table, string[] columns, object[] values, string returningColumnName, string returningParameterName) {
            return String.Format("{0} RETURNING {1}",
                             GetInsertSql(table, columns, values),
                             returningColumnName);
        }

        public override string WrapSelectSqlWithPagination(string sql, int skipRows, int numberOfRows) {
            return String.Format("SELECT * FROM ({0}) AS temp OFFSET {1} LIMIT {2}", sql, skipRows, numberOfRows);
        }

        public override string GetDropIndexSql(string indexName, string table) {
            return String.Format("DROP INDEX {0}", indexName);
        }

        protected override string GetDbTypeString(DbType type, int precision) {
            switch (type) {
                case DbType.AnsiString:
                case DbType.String:
                    if (precision <= 0)
                        return "VARCHAR(255)";
                    if (precision < 10485760)
                        return String.Format("VARCHAR({0})", precision);
                    return "TEXT";
                case DbType.Binary:
                    return "BYTEA";
                case DbType.Boolean:
                    return "BOOL";
                case DbType.Currency:
                    return "MONEY";
                case DbType.Date:
                case DbType.DateTime:
                    return "TIMESTAMP";
                case DbType.Decimal:
                    return precision <= 0 ? "NUMERIC(19,5)" : String.Format("NUMERIC(19,{0})", precision);
                case DbType.Double:
                    return "FLOAT8";
                case DbType.Guid:
                    return "UUID";
                case DbType.Int16:
                    return "SMALLINT";
                case DbType.Int32:
                    return "INTEGER";
                case DbType.Int64:
                    return "BIGINT";
                case DbType.Single:
                    return "FLOAT4";
                case DbType.Time:
                    return "TIME";
                case DbType.AnsiStringFixedLength:
                case DbType.StringFixedLength:
                    return precision <= 0 ? "CHAR(255)" : String.Format("CHAR({0})", precision);
                case DbType.Xml:
                    return "XML";
                case DbType.DateTimeOffset:
                    return "TIMESTAMPTZ";
                default:
                    throw new DataTypeNotAvailableException(String.Format("The type {0} is no available for postgreSql", type));
            }
        }

        public override string GetColumnToSqlWhenCreate(Column col) {
            var colStructure = GetSqlColumnStructure(col);
            return String.Format("{0} {1} {2} {3}", col.ColumnName, colStructure.Type, colStructure.Default, colStructure.Nullable);
        }

        public override string GetColumnValueToSql(object value) {
            if (value is bool) {
                return ((bool)value) ? "true" : "false";
            }

            if ((value is Int16) || (value is Int32) || (value is Int64) || (value is double) || (value is float) || (value is decimal)) {
                return Convert.ToString(value, CultureInfo.InvariantCulture);
            }

            if (value is DateTime) {
                var dt = (DateTime)value;
                return String.Format("'{0}'", dt.ToString("yyyy-MM-ddThh:mm:ss", CultureInfo.InvariantCulture));
            }

            return String.Format("'{0}'", value);
        }

        public override string GetTableExistsSql(string tableName) {
            return String.Format("SELECT COUNT(relname) FROM pg_class WHERE relname = '{0}'", tableName);
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

        public override string GetModifyColumnSql(string tableName, string columnName, Column columnDefinition) {
            var colStructure = GetSqlColumnStructure(columnDefinition);
            var builder = new StringBuilder();
            const string alterColumn = "ALTER COLUMN";

            builder.AppendFormat("ALTER TABLE {0} ", tableName);

            var colType = colStructure.Type;
            if (!String.IsNullOrEmpty(colType)) {
                builder.AppendFormat("{0} {1} TYPE {2} USING {1}::{2},", alterColumn, columnName, colType);
            }

            var colNullable = colStructure.Nullable;
            if (!String.IsNullOrEmpty(colNullable)) {
                var action = colNullable == WordNotNull ? "SET" : "DROP";
                builder.AppendFormat("{0} {1} {2} {3},", alterColumn, columnName, action, WordNotNull);
            }

            var colDefault = colStructure.Default;
            if (!String.IsNullOrEmpty(colDefault)) {
                builder.AppendFormat("{0} {1} SET {2},", alterColumn, columnName, colDefault);
            }

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        private SqlColumnStructure GetSqlColumnStructure(Column col) {
            return new SqlColumnStructure {
                Type = GetDbTypeString(col.Type, col.Size),
                Nullable = col.IsNullable ? WordNull : WordNotNull,
                Default = (col.DefaultValue != null) ? String.Format("DEFAULT {0}", GetColumnValueToSql(col.DefaultValue)) : ""
            };
        }
    }
}