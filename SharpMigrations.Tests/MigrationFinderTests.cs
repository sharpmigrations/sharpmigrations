using System.Linq;
using SharpMigrations;
using Xunit;

namespace SharpMigrations.Tests {
    public class MigrationFinderTests {
        [Theory]
        [InlineData("SeedOne")]
        [InlineData("SEEDONE")]
        [InlineData("seedone")]
        public void Seeds_are_case_insensitive(string seedName) {
            var seed = MigrationFinder.FindSeed(GetType().Assembly, seedName);
            Assert.Equal(typeof(SeedOne), seed);
        }

        [Fact]
        public void Should_find_all_migrations() {
            var migrations = MigrationFinder.FindMigrations(GetType().Assembly);
            Assert.Equal(6, migrations.Count);
            Assert.Equal(typeof(Migration1), migrations.First().MigrationType);
            Assert.Equal(typeof(Migration6), migrations.Last().MigrationType);
        }

        [Fact]
        public void Should_throw_when_seed_not_found() {
            Assert.Throws<SeedNotFoundException>(() => MigrationFinder.FindSeed(GetType().Assembly, "somename"));
        }
    }

    public class SeedOne : SeedMigration {
        public override void Up(string param = null) {
        }
    }
}