using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Databases.Oracle;
using Sharp.Migrations;

namespace Sharp.Tests.Migrations {
    public class SqlToFileDatabaseTests {

        private SqlToFileDatabase _database;

        [SetUp]
        public void SetUp() {
            _database = new SqlToFileDatabase(new OracleDialect());
        }

        [Test]
        public void Record_simple_sql() {
            string sql = "select * from foo";
            _database.ExecuteSql(sql);
            Assert.AreEqual(sql, _database.Sqls[0]);
        }

        [Test]
        public void Record_sql_with_parameter() {
            string sql = "select * from foo where moo = :par0";
            string expected = "select * from foo where moo = 42";
            _database.ExecuteSql(sql, 42);
            Assert.AreEqual(expected, _database.Sqls[0]);
        }
    }
}
