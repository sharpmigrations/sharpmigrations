using System;
using Moq;
using SharpData;
using Xunit;

namespace SharpMigrations.Tests {
	
	public class MigrationFactoryTests {
		private MigrationFactory _factory;
		private IDataClient _dataClient;

		public MigrationFactoryTests() {
			_dataClient = new Mock<IDataClient>().Object;
			_factory = new MigrationFactory(_dataClient);
		}

		[Fact]
		public void Should_create_migration() {
			var migrationType = typeof (Migration1);
			var migration = _factory.CreateMigration(migrationType);
            Assert.True(migration.GetType() == migrationType);
		}

		[Fact]
		public void Created_migration_should_have_a_dataClient() {
			var migrationType = typeof (Migration1);
			var migration = (Migration1) _factory.CreateMigration(migrationType);
			Assert.Equal(_dataClient, migration.DataClient);
		}
	}
}