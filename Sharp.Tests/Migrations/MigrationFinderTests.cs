using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Migrations;

namespace Sharp.Tests.Migrations {
    public class MigrationFinderTests {

        [Test]
        public void Should_find_all_migrations() {
            var migrations = MigrationFinder.FindMigrations(GetType().Assembly);
            Assert.AreEqual(6, migrations.Count);
            Assert.AreEqual(typeof(Migration1), migrations.First().MigrationType);
            Assert.AreEqual(typeof(Migration6), migrations.Last().MigrationType);
        }

        [Test]
        [TestCase("SeedOne")]
        [TestCase("SEEDONE")]
        [TestCase("seedone")]
        public void Seeds_are_case_insensitive(string seedName) {
            var seed = MigrationFinder.FindSeed(GetType().Assembly, seedName);
            Assert.AreEqual(typeof(SeedOne), seed);
        }

        [Test]
        [ExpectedException(typeof(SeedNotFoundException))]
        public void Should_throw_when_seed_not_found() {
            MigrationFinder.FindSeed(GetType().Assembly, "somename");
        }
    }

    public class SeedOne : SeedMigration {
        public override void Up(string param = null) {
        }
    }
}
