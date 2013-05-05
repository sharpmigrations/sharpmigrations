using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Sharp.Data.Schema;

namespace Sharp.Tests.Data.Schema {
    [TestFixture]
    public class TableTests {
       
        [Test]
        public void ConstructorTest() {
            var table = new Table("name");
            Assert.AreEqual("name", table.Name);
            Assert.IsNotNull(table.Columns);
            Assert.AreEqual(0, table.Columns.Count);
        }
    }
}
