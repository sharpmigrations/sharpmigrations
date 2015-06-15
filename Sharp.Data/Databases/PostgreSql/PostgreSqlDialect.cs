using System;
using System.Data;
using Sharp.Data.Schema;

namespace Sharp.Data.Databases.PostgreSql {
    public sealed class PostgreSqlDialect : Dialect {
        private readonly PostgreSqlTableDialect _postgreSqlTableDialect;
        private readonly PostgreSqlConstraintsDialect _postgreSqlConstraintsDialect;
        private readonly PostgreSqlColumnDialect _postgreSqlColumnDialect;

        public PostgreSqlDialect() {
            _postgreSqlTableDialect = new PostgreSqlTableDialect();
            _postgreSqlColumnDialect = new PostgreSqlColumnDialect(WordNull, WordNotNull, PostgreSqlDbTypesDialect.GetDbTypeString);
            _postgreSqlConstraintsDialect = new PostgreSqlConstraintsDialect();
        }

        public override string ParameterPrefix {
            get { return ":"; }
        }

        public override DbType GetDbType(string sqlType, int dataPrecision) {
            throw new NotImplementedException();
        }

        public override string[] GetCreateTableSqls(Table table) {
            return _postgreSqlTableDialect.GetCreateTableSqls(table, GetColumnToSqlWhenCreate, GetPrimaryKeySql);
        }

        public override string[] GetDropTableSqls(string tableName) {
            return _postgreSqlTableDialect.GetDropTableSqls(tableName);
        }

        public override string GetForeignKeySql(string fkName, string table, string column, string referencingTable, string referencingColumn, OnDelete onDelete) {
            return _postgreSqlConstraintsDialect.GetForeignKeySql(fkName, table, column, referencingTable, referencingColumn, onDelete);
        }

        public override string GetUniqueKeySql(string ukName, string table, params string[] columnNames) {
            return _postgreSqlConstraintsDialect.GetUniqueKeySql(ukName, table, columnNames);
        }

        public override string GetDropUniqueKeySql(string uniqueKeyName, string tableName) {
            return _postgreSqlConstraintsDialect.GetDropUniqueKeySql(uniqueKeyName, tableName);
        }

        protected override string GetDbTypeString(DbType type, int precision) {
            return PostgreSqlDbTypesDialect.GetDbTypeString(type, precision);
        }

        public override string GetColumnToSqlWhenCreate(Column col) {
            return _postgreSqlColumnDialect.GetColumnToSqlWhenCreate(col);
        }

        public override string GetColumnValueToSql(object value) {
            return _postgreSqlColumnDialect.GetColumnValueToSql(value);
        }

        public override string GetTableExistsSql(string tableName) {
            return _postgreSqlTableDialect.GetTableExistsSql(tableName);
        }

        public override string GetAddCommentToColumnSql(string tableName, string columnName, string comment) {
            return _postgreSqlColumnDialect.GetAddCommentToColumnSql(tableName, columnName, comment);
        }

        public override string GetAddCommentToTableSql(string tableName, string comment) {
            return _postgreSqlTableDialect.GetAddCommentToTableSql(tableName, comment);
        }

        public override string GetRemoveCommentFromColumnSql(string tableName, string columnName) {
            return _postgreSqlColumnDialect.GetRemoveCommentToColumnSql(tableName, columnName);
        }

        public override string GetRemoveCommentFromTableSql(string tableName) {
            return _postgreSqlTableDialect.GetRemoveCommentToTableSql(tableName);
        }

        public override string GetRenameTableSql(string tableName, string newTableName) {
            return _postgreSqlTableDialect.GetRenameTableSql(tableName, newTableName);
        }

        public override string GetRenameColumnSql(string tableName, string columnName, string newColumnName) {
            return _postgreSqlColumnDialect.GetRenameColumnSql(tableName, columnName, newColumnName);
        }

        public override string GetModifyColumnSql(string tableName, string columnName, Column columnDefinition) {
            return _postgreSqlColumnDialect.GetModifyColumnSql(tableName, columnName, columnDefinition);
        }

        public override string GetInsertReturningColumnSql(string table, string[] columns, object[] values, string returningColumnName, string returningParameterName) {
            return String.Format("{0} RETURNING {1}", GetInsertSql(table, columns, values), returningColumnName);
        }

        public override string WrapSelectSqlWithPagination(string sql, int skipRows, int numberOfRows) {
            return String.Format("SELECT * FROM ({0}) AS temp OFFSET {1} LIMIT {2}", sql, skipRows, numberOfRows);
        }

        public override string GetDropIndexSql(string indexName, string table) {
            return String.Format("DROP INDEX {0}", indexName);
        }
    }
}