using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Data;
using Sharp.Data.Schema;

namespace Sharp.Tests.Data.Schema {
    [TestFixture]
    public class ColumnTests {
       
        [Test]
        public void Constructor_with_no_type_test() {
            var c = new Column("COL1");
            Assert.AreEqual(DbType.String, c.Type);
            TestDefaults(c);
        }

        [Test]
        public void Constructor_with_type_test() {
            var c = new Column("COL1", DbType.UInt32);
            Assert.AreEqual(DbType.UInt32, c.Type);
            TestDefaults(c);
        }

        [Test]
        public void When_column_is_autoIncrement_it_is_also_not_null() {
            var c = new Column("COL1", DbType.UInt32);
            c.IsAutoIncrement = true;
            Assert.IsFalse(c.IsNullable);
        }

        [Test]
        public void When_column_is_primary_key_it_is_also_not_null() {
            var c = new Column("COL1", DbType.UInt32);
            c.IsPrimaryKey = true;
            Assert.IsFalse(c.IsNullable);
        }

        protected void TestDefaults(Column c) {
            Assert.IsNull(c.DefaultValue);
            Assert.AreEqual("COL1", c.ColumnName);
            Assert.IsFalse(c.IsAutoIncrement);
            Assert.IsTrue(c.IsNullable);
        }
    }
}
