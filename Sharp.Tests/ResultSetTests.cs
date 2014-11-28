using NUnit.Framework;
using Sharp.Data;

namespace Sharp.Tests {
    public class ResultSetTests {
        [Test]
        public void Should_return_empty() {
            var res = new ResultSet("c1");
            Assert.IsTrue(res.IsEmpty);
        }

        [Test]
        public void Should_not_return_empty() {
            var res = new ResultSet("c1");
            res.AddRow(1);
            Assert.IsFalse(res.IsEmpty);
        }
    }
}