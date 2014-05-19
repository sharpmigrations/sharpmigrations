using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Sharp.Data;

namespace Sharp.Tests.Data {
    public class DataReaderToResultSetMapperTests {

        [Test]
        public void Can_map_selects_with_columns() {
            var reader = new Mock<IDataReader>();
            reader.Setup(x => x.GetName(0)).Returns("col1");
            reader.Setup(x => x.GetName(1)).Returns("col2");
            reader.Setup(x => x.FieldCount).Returns(2);
            var result = DataReaderToResultSetMapper.Map(reader.Object);
            CollectionAssert.AreEqual(new[] { "col1", "col2" }, result.GetColumnNames());
        }

        [Test]
        public void Can_map_selects_with_two_columns_with_the_same_name() {
            var reader = new Mock<IDataReader>();
            reader.Setup(x => x.GetName(0)).Returns("col");
            reader.Setup(x => x.GetName(1)).Returns("colx");
            reader.Setup(x => x.GetName(2)).Returns("col");
            reader.Setup(x => x.FieldCount).Returns(3);
            var result = DataReaderToResultSetMapper.Map(reader.Object);
            CollectionAssert.AreEqual(new [] {"col", "colx", "col_2"}, result.GetColumnNames());
        }
    }
}
