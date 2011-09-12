using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Migrations;

namespace Sharp.Tests.Migrations {
	[TestFixture]
	public class MigratorTests {
		private Migrator _migrator;
		private List<Migration> _migrationsToRun;

		[SetUp]
		public void SetUp() {
			MigrationTestHelper.Clear();
			_migrationsToRun = CreateMigrationsList();
		}

		private List<Migration> CreateMigrationsList() {
			return new List<Migration> {
				new Migration1(),
				new Migration2(),
				new Migration3()
			};
		}

		[Test]
		public void Should_run_migrations_from_0_to_3() {
			_migrator = new Migrator(_migrationsToRun);
			_migrator.Migrate();
			
			Assert.AreEqual(3, MigrationTestHelper.ExecutedMigrationsUp.Count);
		}

		[Test]
		public void Should_run_migrations_from_3_to_0() {
			_migrationsToRun.Reverse();
			_migrator = new Migrator(_migrationsToRun);
			_migrator.Migrate();

			Assert.AreEqual(3, MigrationTestHelper.ExecutedMigrationsDown.Count);
		}

		[Test]
		public void Should_inform_last_executed_migration_version() {
			_migrator = new Migrator(_migrationsToRun);
			_migrator.Migrate();

			Assert.AreEqual(3, _migrator.CurrentVersion);
		}

		[Test]
		public void Should_inform_last_executed_migration_version_even_on_exception() {

			MigrationTestHelper.VersionForException = 6;

			_migrator = new Migrator(_migrationsToRun);
			try {
				_migrator.Migrate();
			}
			catch {}

			Assert.AreEqual(3, _migrator.CurrentVersion);
		}
	}
}
