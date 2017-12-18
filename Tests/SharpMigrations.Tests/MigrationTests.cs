using Moq;
using SharpData;
using Xunit;

namespace SharpMigrations.Tests {

    
    public class MigrationTests {
        private Mock<IDataClient> _dataClientMock;
        private Mock<IDatabase> _databaseMock;
        private Migration1 _migration1;

        public MigrationTests() {
            _dataClientMock = new Mock<IDataClient>();
            _databaseMock = new Mock<IDatabase>();
            _dataClientMock.SetupGet(d => d.Database).Returns(_databaseMock.Object);
            _migration1 = new Migration1();
            _migration1.SetDataClient(_dataClientMock.Object);
        }

        [Fact]
        public void Should_execute_sql() {
            const string sql = "sql";
            var @params = new[] {1, 2, 3};
            _migration1.ExecuteSql(sql, @params);
            _databaseMock.Verify(d => d.ExecuteSql(sql, @params));
        }
    }
}