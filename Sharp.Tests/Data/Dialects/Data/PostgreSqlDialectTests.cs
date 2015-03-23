using System;
using NUnit.Framework;
using Sharp.Data.Databases.PostgreSql;

namespace Sharp.Tests.Databases.PostgreSql {
    public class PostgreSqlDialectTests : DialectDataTests {
        [SetUp]
        public void SetUp() {
            _dialect = new PostgreSqlDialect();
        }

        protected override string GetResultFor_Can_create_check_if_table_exists_sql() {
            return String.Format("SELECT relname FROM pg_class WHERE relname = '{0}'", TABLE_NAME);
        }

        protected override string GetResultFor_Can_generate_count_sql() {
            return "SELECT COUNT(*) FROM MYTABLE";
        }

        protected override string GetResultFor_Can_generate_select_sql_with_pagination(int skip, int to) {
            var sql = GetSelectAllSql();
            return String.Format("SELECT * FROM ({0}) OFFSET {1} LIMIT {2}", sql, skip, to);
        }
    }
}
