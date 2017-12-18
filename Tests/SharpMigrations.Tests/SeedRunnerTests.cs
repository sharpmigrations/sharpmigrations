using Moq;
using SharpData;
using SharpMigrations.Runners;
using Xunit;

namespace SharpMigrations.Tests {
    public class SeedRunnerTests {
        private SeedRunner _seedRunner;

        public SeedRunnerTests() {
            SeedForTest.Reset();
            var client = new Mock<IDataClient>();
            _seedRunner = new SeedRunner(client.Object, GetType().Assembly);
        }

        [Fact]
        public void Should_throw_when_seed_not_found() {
            Assert.Throws<SeedNotFoundException>(() => _seedRunner.Run("someseed"));
        }

        [Fact]
        public void Should_call_run() {
            _seedRunner.Run("SeedForTest");
            Assert.True(SeedForTest.UpCalled);
        }

        [Fact]
        public void Should_pass_parameter() {
            _seedRunner.Run("SeedForTest", "myParam");
            Assert.Equal("myParam", SeedForTest.Param);          
        }
    }
}