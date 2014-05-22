using System;
using System.Linq;
using NUnit.Framework;
using Sharp.Data;

namespace Sharp.Tests.Data {
    public class ResultSetExtensionsTests {
        [Test]
        public void Should_map_object() {
            var res = CreateResultSet();
            var list = res.Map<SomeClass>();

            Assert.AreEqual(1, list[0].Int);
            Assert.AreEqual("String", list[0].String);
            Assert.AreEqual(DateTime.Today, list[0].DateTime);
            Assert.IsTrue(list[0].Double - 1.1 < Double.Epsilon);

            Assert.AreEqual(2, list[1].Int);
            Assert.AreEqual("String2", list[1].String);
            Assert.AreEqual(DateTime.Today, list[1].DateTime);
            Assert.IsTrue(list[1].Double - 1.2 < Double.Epsilon);
        }

        [Test]
        public void Should_map_object_with_extra_properties() {
            var res = CreateResultSet();
            var list = res.Map<ExtraProperties>();

            Assert.AreEqual(1, list[0].Int);
            Assert.AreEqual("String", list[0].String);
            Assert.AreEqual(DateTime.Today, list[0].DateTime);
            Assert.IsTrue(list[0].Double - 1.1 < Double.Epsilon);
        }


        [Test]
        public void Should_map_object_with_missing_properties() {
            var res = CreateResultSet();
            var list = res.Map<MissingProperties>();
            Assert.AreEqual(1, list[0].Int);
        }

        [Test]
        public void Should_map_object_with_different_type() {
            var res = CreateResultSet();
            var list = res.Map<WrongType>();
            Assert.AreEqual("1", list[0].Int);
        }

        [Test]
        public void Should_map_private_property() {
            var res = CreateResultSet();
            var list = res.Map<PrivateSet>();
            Assert.AreEqual(1, list[0].Int);
        }

        [Test]
        public void Should_map_nullable_prop() {
            var res = CreateResultSet();
            var list = res.Map<NullableProp>();
            Assert.AreEqual(1, list[0].Int);
        }

        [Test]
        public void Should_map_null_value_to_nullable_prop() {
            var res = CreateResultSet();
            res.AddRow(null, "String", DateTime.Today, 1.1);
            var list = res.Map<NullableProp>();
            Assert.IsNull(list.Last().Int);
        }

        [Test]
        public void Should_map_null_value() {
            var res = CreateResultSet();
            res.AddRow(1, null, DateTime.Today, 1.1);
            var list = res.Map<SomeClass>();
            Assert.IsNull(list.Last().String);
        }

        private static ResultSet CreateResultSet() {
            var res = new ResultSet("Int", "String", "DateTime", "Double");
            res.AddRow(1, "String", DateTime.Today, 1.1);
            res.AddRow(2, "String2", DateTime.Today, 1.2);
            return res;
        }

        private class SomeClass {
            public int Int { get; set; }
            public string String { get; set; }
            public DateTime DateTime { get; set; }
            public double Double { get; set; }
        }

        private class ExtraProperties {
            public int Int { get; set; }
            public string String { get; set; }
            public DateTime DateTime { get; set; }
            public double Double { get; set; }
            public string String2 { get; set; }
        }

        private class NullableProp {
            public int? Int { get; set; }
        }

        private class MissingProperties {
            public int Int { get; set; }
        }

        private class PrivateSet {
            public int Int { get; private set; }
        }

        private class WrongType {
            public string Int { get; private set; }
        }
    }
}