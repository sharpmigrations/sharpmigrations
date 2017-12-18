using Moq;
using SharpData;
using SharpData.Fluent;
using Xunit;

namespace SharpMigrations.Tests {
    public class VersionRepostoryTests {

        [Fact]
        public void Should_set_default_migration_group() {
            var dataClientMock = new Mock<IDataClient>();
            dataClientMock.Setup(d => d.Add).Returns(new FluentAdd(dataClientMock.Object));
            var rep = new VersionRepository(dataClientMock.Object, null);
            Assert.Equal(VersionRepository.DEFAULT_MIGRATION_GROUP, rep.MigrationGroup);
        }
    }
}