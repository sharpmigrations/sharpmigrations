using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Data;
using Sharp.Data.Schema;

namespace Sharp.Tests.Data.Schema {
    [TestFixture]
    public class FluentColumnTest {

        [Test]
        public void FluentCreationTests() {
            string col = "COL1";

            FluentColumn f = Column.WithName(col).WithType.Binary;
            Assert.AreEqual(col, f.Object.ColumnName);
            Assert.AreEqual(DbType.Binary, f.Object.Type);

            f = Column.String(col);
            Assert.AreEqual(col, f.Object.ColumnName);
            Assert.AreEqual(DbType.String, f.Object.Type);

            f = Column.Int16(col);
            Assert.AreEqual(col, f.Object.ColumnName);
            Assert.AreEqual(DbType.Int16, f.Object.Type);

            f = Column.Int32(col);
            Assert.AreEqual(col, f.Object.ColumnName);
            Assert.AreEqual(DbType.Int32, f.Object.Type);

            f = Column.Int64(col);
            Assert.AreEqual(col, f.Object.ColumnName);
            Assert.AreEqual(DbType.Int64, f.Object.Type);

            f = Column.Boolean(col);
            Assert.AreEqual(col, f.Object.ColumnName);
            Assert.AreEqual(DbType.Boolean, f.Object.Type);

            f = Column.Binary(col);
            Assert.AreEqual(col, f.Object.ColumnName);
            Assert.AreEqual(DbType.Binary, f.Object.Type);

            f = Column.Date(col);
            Assert.AreEqual(col, f.Object.ColumnName);
            Assert.AreEqual(DbType.Date, f.Object.Type);

            f = Column.Decimal(col);
            Assert.AreEqual(col, f.Object.ColumnName);
            Assert.AreEqual(DbType.Decimal, f.Object.Type);

            f = Column.Single(col);
            Assert.AreEqual(col, f.Object.ColumnName);
            Assert.AreEqual(DbType.Single, f.Object.Type);

            f = Column.Double(col);
            Assert.AreEqual(col, f.Object.ColumnName);
            Assert.AreEqual(DbType.Double, f.Object.Type);

            f = Column.Guid(col);
            Assert.AreEqual(col, f.Object.ColumnName);
            Assert.AreEqual(DbType.Guid, f.Object.Type);
        }

        [Test]
        public void Fluent_can_set_default_value() {
            Column c = Column.String("COL").DefaultValue("foo").Object;
            Assert.AreEqual("foo", c.DefaultValue);
        }

        [Test]
        public void Fluent_can_set_autoIncrement() {
            Column c = Column.AutoIncrement("COL").Object;
            Assert.IsTrue(c.IsAutoIncrement);
        }

        [Test]
        public void Fluent_can_set_is_not_null() {
            Column c = Column.String("COL").NotNull().Object;
            Assert.IsFalse(c.IsNullable);
        }

        [Test]
        public void ComplexExample() {
            Column c = Column.String("COL").DefaultValue("foo").NotNull().AsPrimaryKey().Object;
            Assert.AreEqual("COL", c.ColumnName);
            Assert.AreEqual("foo", c.DefaultValue);
            Assert.IsTrue(c.IsPrimaryKey);
            Assert.IsFalse(c.IsNullable);
        }
    }
}
