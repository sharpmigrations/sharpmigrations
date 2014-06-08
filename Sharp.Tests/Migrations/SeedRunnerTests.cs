using Moq;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Migrations;

namespace Sharp.Tests.Migrations {
    public class SeedRunnerTests {
        private SeedRunner _seedRunner;

        [SetUp]
        public void SetUp() {
            SeedForTest.Reset();
            var client = new Mock<IDataClient>();
            _seedRunner = new SeedRunner(client.Object, GetType().Assembly);
        }

        [Test]
        [ExpectedException(typeof (SeedNotFoundException))]
        public void Should_throw_when_seed_not_found() {
            _seedRunner.Run("someseed");
        }

        [Test]
        public void Should_call_run() {
            _seedRunner.Run("SeedForTest");
            Assert.IsTrue(SeedForTest.UpCalled);
        }

        [Test]
        public void Should_pass_parameter() {
            _seedRunner.Run("SeedForTest", "myParam");
            Assert.AreEqual("myParam", SeedForTest.Param);          
        }
    }
}