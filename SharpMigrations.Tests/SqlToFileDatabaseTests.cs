using SharpData.Databases.Oracle;
using SharpMigrations.Runners.ScriptCreator;
using Xunit;

namespace SharpMigrations.Tests {
    public class SqlToFileDatabaseTests {
        private ScriptCreatorDatabase _database;

        public SqlToFileDatabaseTests() {
            _database = new ScriptCreatorDatabase(new OracleDialect(), null);
        }

        [Fact]
        public void Record_simple_sql() {
            var sql = "select * from foo";
            _database.ExecuteSql(sql);
            Assert.Equal(sql, _database.Sqls[0]);
        }

        [Fact]
        public void Record_sql_with_parameter() {
            var sql = "select * from foo where moo = :par0";
            var expected = "select * from foo where moo = 42";
            _database.ExecuteSql(sql, 42);
            Assert.Equal(expected, _database.Sqls[0]);
        }
    }
}