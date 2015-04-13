using System;
using System.Data;
using System.Globalization;
using System.Text;
using Sharp.Data.Schema;

namespace Sharp.Data.Databases.PostgreSql {
    internal class PostgreSqlColumnDialect {
        private readonly string _wordNull;
        private readonly string _wordNotNull;
        private readonly Func<DbType, int, string> _getDbTypeString;

        public PostgreSqlColumnDialect(string wordNull, string wordNotNull, Func<DbType, int, string> getDbTypeString) {
            _wordNull = wordNull;
            _wordNotNull = wordNotNull;
            _getDbTypeString = getDbTypeString;
        }

        public string GetColumnToSqlWhenCreate(Column col) {
            var colStructure = GetSqlColumnStructure(col, _getDbTypeString);
            return String.Format("{0} {1} {2} {3}", col.ColumnName, colStructure.Type, colStructure.Default, colStructure.Nullable);
        }

        public string GetColumnValueToSql(object value) {
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

        public string GetRenameColumnSql(string tableName, string columnName, string newColumnName) {
            return String.Format("ALTER TABLE {0} RENAME COLUMN {1} TO {2}", tableName, columnName, newColumnName);
        }

        public string GetModifyColumnSql(string tableName, string columnName, Column columnDefinition) {
            var colStructure = GetSqlColumnStructure(columnDefinition, _getDbTypeString);
            var builder = new StringBuilder();
            const string alterColumn = "ALTER COLUMN";

            builder.AppendFormat("ALTER TABLE {0} ", tableName);

            GetTypeSqlToRenameColumnCommand(columnName, colStructure, builder, alterColumn);
            GetNullableSqlToRenameColumnCommand(columnName, colStructure, builder, alterColumn);
            GetDefaultValueSqlToRenameColumnCommand(columnName, colStructure, builder, alterColumn);

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        private static void GetDefaultValueSqlToRenameColumnCommand(string columnName, SqlColumnStructure colStructure, StringBuilder builder, string alterColumn) {
            var colDefault = colStructure.Default;
            if (!String.IsNullOrEmpty(colDefault)) {
                builder.AppendFormat("{0} {1} SET {2},", alterColumn, columnName, colDefault);
            }
        }

        private void GetNullableSqlToRenameColumnCommand(string columnName, SqlColumnStructure colStructure, StringBuilder builder, string alterColumn) {
            var colNullable = colStructure.Nullable;
            if (!String.IsNullOrEmpty(colNullable)) {
                var action = colNullable == _wordNotNull ? "SET" : "DROP";
                builder.AppendFormat("{0} {1} {2} {3},", alterColumn, columnName, action, _wordNotNull);
            }
        }

        private static void GetTypeSqlToRenameColumnCommand(string columnName, SqlColumnStructure colStructure, StringBuilder builder, string alterColumn) {
            var colType = colStructure.Type;
            if (!String.IsNullOrEmpty(colType)) {
                builder.AppendFormat("{0} {1} TYPE {2} USING {1}::{2},", alterColumn, columnName, colType);
            }
        }

        public string GetAddCommentToColumnSql(string tableName, string columnName, string comment) {
            return String.Format("COMMENT ON COLUMN {0}.{1} IS '{2}'", tableName, columnName, comment);
        }

        public string GetRemoveCommentToColumnSql(string tableName, string columnName) {
            return String.Format("COMMENT ON COLUMN {0}.{1} IS ''", tableName, columnName);
        }

        private SqlColumnStructure GetSqlColumnStructure(Column col, Func<DbType, int, string> getDbTypeString) {
            return new SqlColumnStructure {
                Type = getDbTypeString(col.Type, col.Size),
                Nullable = col.IsNullable ? _wordNull : _wordNotNull,
                Default = (col.DefaultValue != null) ? String.Format("DEFAULT {0}", GetColumnValueToSql(col.DefaultValue)) : ""
            };
        }
    }
}