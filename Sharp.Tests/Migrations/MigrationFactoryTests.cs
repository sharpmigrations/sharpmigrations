using System;
using Moq;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Migrations;

namespace Sharp.Tests.Migrations {
	[TestFixture]
	public class MigrationFactoryTests {
		private MigrationFactory _factory;
		private IDataClient _dataClient;

		[SetUp]
		public void Init() {
			_dataClient = new Mock<IDataClient>().Object;
			_factory = new MigrationFactory(_dataClient);
		}

		[Test]
		public void Should_create_migration() {
			Type migrationType = typeof (Migration1);
			Migration migration = _factory.CreateMigration(migrationType);
			Assert.IsInstanceOf<Migration1>(migration);;
		}

		[Test]
		public void Created_migration_should_have_a_dataClient() {
			Type migrationType = typeof (Migration1);
			var migration = (Migration1) _factory.CreateMigration(migrationType);
			Assert.AreEqual(_dataClient, migration.DataClient);
		}
	}
}