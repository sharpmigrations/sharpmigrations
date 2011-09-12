using System;
using Moq;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Migrations;

namespace Sharp.Tests.Migrations {
	[TestFixture]
	public class VersionRepositoryTests {
		private Mock<IDataClient> _dataClient;
		private VersionRepository _versionRepository;


		[SetUp]
		public void SetUp() {
			_dataClient = new Mock<IDataClient>();
			
		}

		[Test]
		[ExpectedException(typeof (MigrationException))]
		public void Should_throw_migration_exception_when_cant_create_version_table() {
			_dataClient.Setup(p => p.TableExists(It.IsAny<string>())).Throws(new Exception("foo"));
            
            _versionRepository = new VersionRepository(_dataClient.Object);
		}
	}
}