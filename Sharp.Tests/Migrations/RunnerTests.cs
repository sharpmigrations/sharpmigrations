using System;
using System.Collections.Generic;
using Moq;
using System.Linq;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Config;
using Sharp.Migrations;
using System.Reflection;

namespace Sharp.Tests.Migrations {
	[TestFixture]
	public class RunnerTests {
		private Runner _runner;
		private Mock<IDataClient> _dataClient;
		private Mock<IVersionRepository> _versionRepository;

		[SetUp]
		public void SetUp() {
			MigrationTestHelper.Clear();

			_dataClient = new Mock<IDataClient>();
		    _dataClient.Setup(p => p.TableExists(VersionRepository.VERSION_TABLE_NAME)).Returns(true);

            _versionRepository = new Mock<IVersionRepository>();
            
			_runner = new Runner(_dataClient.Object, Assembly.GetExecutingAssembly());
			_runner.VersionRepository = _versionRepository.Object;

			DefaultConfig.IgnoreDialectNotSupportedActions = false;
		}

		[Test]
		[ExpectedException(typeof(MigrationException))]
		public void Should_throw_migration_exception_when_cant_run_migration() {
			MigrationTestHelper.VersionForException = 1;
			_runner.Run(5);
		}

		[Test]
		public void When_going_from_0_to_5__update_database_version_with_5() {
			_runner.Run(5);
			_versionRepository.Verify(p => p.UpdateVersion(5));
		}

		[Test]
		public void When_going_from_0_to_6_and_exception_on_6__update_database_with_5() {
			MigrationTestHelper.VersionForException = 6;
			try {
				_runner.Run(6);
			}
			catch {}
			_versionRepository.Verify(p => p.UpdateVersion(5));
		}

		[Test]
		public void When_going_from_6_to_0_and_exception_on_4__update_database_with_4() {
			MigrationTestHelper.VersionForException = 4;
			_versionRepository.Setup(p => p.GetCurrentVersion()).Returns(6);			
			try {
				_runner.Run(0);
			}
			catch { }
			_versionRepository.Verify(p => p.UpdateVersion(4));
		}

		[Test]
		public void When_going_from_5_to_0__update_database_version_with_0() {
			_versionRepository.Setup(p => p.GetCurrentVersion()).Returns(5);			
			_runner.Run(0);
			_versionRepository.Verify(p => p.UpdateVersion(0));
		}

		[Test]
		public void When_going_from_5_to_1__update_database_version_with_1() {
			_versionRepository.Setup(p => p.GetCurrentVersion()).Returns(5);
			_runner.Run(1);
			_versionRepository.Verify(p => p.UpdateVersion(1));
		}

		[Test]
		public void When_going_down_from_3_to_1__dont_execute_the_1_down() {
			_versionRepository.Setup(p => p.GetCurrentVersion()).Returns(3);
			_runner.Run(1);

			Assert.AreEqual(2, MigrationTestHelper.ExecutedMigrationsDown.Count);
		}

		[Test]
		public void When_going_up_from_1_to_3__dont_execute_the_1_up() {
			_versionRepository.Setup(p => p.GetCurrentVersion()).Returns(1);
			_runner.Run(3);

			Assert.AreEqual(2, MigrationTestHelper.ExecutedMigrationsUp.Count);

			List<Type> types = MigrationTestHelper.ExecutedMigrationsUp.Select(p => p.GetType()).ToList();
			
			CollectionAssert.DoesNotContain(types, typeof(Migration1));
		}

		[Test]
		public void When_going_up_from_0_to_3__execute_3_ups() {
			_runner.Run(3);
			Assert.AreEqual(3, MigrationTestHelper.ExecutedMigrationsUp.Count);
			Assert.AreEqual(typeof(Migration3), MigrationTestHelper.ExecutedMigrationsUp.Last().GetType());
		}

		[Test]
		public void When_going_up_from_0_to_6__execute_6_ups() {
			_runner.Run(6);
			Assert.AreEqual(6, MigrationTestHelper.ExecutedMigrationsUp.Count);
			Assert.AreEqual(typeof(Migration6), MigrationTestHelper.ExecutedMigrationsUp.Last().GetType());
		}

		[Test]
		public void Negative_target_version_means_last_version() {
			
			_runner.Run(-1);

			int maxVersion = VersionHelper.GetVersion(MigrationTestHelper.GetMigrations().Last());

			Assert.AreEqual(maxVersion, MigrationTestHelper.ExecutedMigrationsUp.Last().Version);
		}

		[Test]
		public void When_exception_should_call_rollback() {
			MigrationTestHelper.VersionForException = 4;
			try {
				_runner.Run(-1);
			}
			catch {}
			_dataClient.Verify(p => p.RollBack());
		}

		[Test]
		public void Should_not_throw_not_supported_by_dialect_when_config_is_set() {
			DefaultConfig.IgnoreDialectNotSupportedActions = true;
			MigrationTestHelper.VersionForNotSupportedByDialectException = 4;
			_runner.Run(5);
		}

		[Test]
		[ExpectedException(typeof(NotSupportedByDialect))]
		public void Should_throw_not_supported_by_dialect_when_config_is_set() {
			DefaultConfig.IgnoreDialectNotSupportedActions = false;
			MigrationTestHelper.VersionForNotSupportedByDialectException = 4;
			_runner.Run(5);
		}

		[TearDown]
		public void TearDown() {
			DefaultConfig.IgnoreDialectNotSupportedActions = false;
		}
	}
}