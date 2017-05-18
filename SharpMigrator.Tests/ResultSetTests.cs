using Sharp.Data;
using Xunit;

namespace SharpMigrator.Tests {
    public class ResultSetTests {
        [Fact]
        public void Should_return_empty() {
            var res = new ResultSet("c1");
            Assert.True(res.IsEmpty);
        }

        [Fact]
        public void Should_not_return_empty() {
            var res = new ResultSet("c1");
            res.AddRow(1);
            Assert.False(res.IsEmpty);
        }
    }
}