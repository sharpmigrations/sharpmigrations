using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Data.Schema;
using Sharp.Migrations;
using Sharp.Migrations.Runners;

namespace Sharp.Tests.Databases.Migrations {
    public class VersionRepositoryTests {

        private VersionRepository _versionRepository;
        private IDataClient _client;
        

        [SetUp]
        public void SetUp() {
            _client = DBBuilder.GetDataClient(DataProviderNames.OracleManaged);
            _versionRepository = new VersionRepository(_client);
            if (_client.TableExists(VersionRepository.VERSION_TABLE_NAME)) {
                _client.RemoveTable(VersionRepository.VERSION_TABLE_NAME);
            }
            if (_client.TableExists(VersionRepository.OLD_VERSION_TABLE_NAME)) {
                _client.RemoveTable(VersionRepository.OLD_VERSION_TABLE_NAME);
            }
        }

        [TearDown]
        public void TearDown() {
            _client.Dispose();
        }

        [Test]
        public void Should_ensure_schema_version_table_existence() {
            _versionRepository.EnsureSchemaVersionTable(AllMigrations);
            Assert.IsTrue(_client.TableExists(VersionRepository.VERSION_TABLE_NAME));
        }

        [Test]
        public void Should_insert_versions_migrating_up() {
            _versionRepository.EnsureSchemaVersionTable(AllMigrations);
            _versionRepository.InsertVersion(M1);
            _versionRepository.InsertVersion(M2);
            _versionRepository.InsertVersion(M3);

            List<VersionInfo> all = QueryVersionTable();
            Assert.AreEqual(3, all.Count);
            for (int i = 0; i < 3; i++) {
                Assert.AreEqual((i+1), all[i].Version);
                Assert.AreEqual("Migration" + (i + 1), all[i].Info);
                Assert.AreEqual("default", all[i].MigrationGroup);
            }
        }

        [Test]
        public void Should_delete_versions_migrating_down() {
            _versionRepository.EnsureSchemaVersionTable(AllMigrations);
            _versionRepository.InsertVersion(M1);
            _versionRepository.InsertVersion(M2);
            _versionRepository.InsertVersion(M3);

            _versionRepository.RemoveVersion(M3);

            List<VersionInfo> all = QueryVersionTable();
            Assert.AreEqual(2, all.Count);
            for (int i = 0; i < 2; i++) {
                Assert.AreEqual((i + 1), all[i].Version);
                Assert.AreEqual("Migration" + (i+1), all[i].Info);
                Assert.AreEqual("default", all[i].MigrationGroup);
            }
        }

        [Test]
        public void Should_migrate_from_old_version_table_to_new_version_table() {
            CreateOldVersionTable();
            InsertOldVersionTableVersionNumber(3);
            _versionRepository.EnsureSchemaVersionTable(AllMigrations);

            List<VersionInfo> all = QueryVersionTable();
            Assert.AreEqual(3, all.Count);
            for (int i = 0; i < 3; i++) {
                Assert.AreEqual((i + 1), all[i].Version);
                Assert.AreEqual("Migration" + (i + 1), all[i].Info);
                Assert.AreEqual("default", all[i].MigrationGroup);
            }
        }

        [Test]
        public void Old_version_table_is_destroyed_after_migration() {
            CreateOldVersionTable();
            InsertOldVersionTableVersionNumber(3);
            _versionRepository.EnsureSchemaVersionTable(AllMigrations);
            Assert.IsFalse(_client.TableExists(VersionRepository.OLD_VERSION_TABLE_NAME));
        }

        [Test]
        public void Should_deal_with_multiple_migration_groups__GetCurrentVersion() {
            _versionRepository.EnsureSchemaVersionTable(AllMigrations);
            _versionRepository.InsertVersion(M1);
            _versionRepository.InsertVersion(M2);
            _versionRepository.InsertVersion(M3);
            Assert.AreEqual(3, _versionRepository.GetCurrentVersion());
            
            _versionRepository.MigrationGroup = "group2";
            _versionRepository.InsertVersion(M1);
            Assert.AreEqual(1, _versionRepository.GetCurrentVersion());
        }

        [Test]
        public void Should_deal_with_multiple_migration_groups__GetAppliedMigrations() {
            _versionRepository.EnsureSchemaVersionTable(AllMigrations);
            _versionRepository.InsertVersion(M1);
            _versionRepository.InsertVersion(M2);
            _versionRepository.InsertVersion(M3);
            Assert.AreEqual(3, _versionRepository.GetAppliedMigrations().Count);

            _versionRepository.MigrationGroup = "group2";
            _versionRepository.InsertVersion(M1);
            Assert.AreEqual(1, _versionRepository.GetAppliedMigrations().Count);
        }

        private void CreateOldVersionTable() {
            if (_client.TableExists(VersionRepository.OLD_VERSION_TABLE_NAME)) {
                _client.RemoveTable(VersionRepository.OLD_VERSION_TABLE_NAME);
            }
            _client.Add
                .Table(VersionRepository.OLD_VERSION_TABLE_NAME)
                .WithColumns(
                    Column.String("name").Size(200).AsPrimaryKey(),
                    Column.Int64("version")
                );
        }

        private void InsertOldVersionTableVersionNumber(int number, string group = "default") {
            _client.Insert.Into(VersionRepository.OLD_VERSION_TABLE_NAME)
                          .Columns("name", "version")
                          .Values(group, number);
        }

        private List<VersionInfo> QueryVersionTable() {
            return _client.Select.AllColumns().From(VersionRepository.VERSION_TABLE_NAME).AllRows().Map<VersionInfo>();
        }

        public static MigrationInfo M1 = new MigrationInfo(typeof(Migration1));
        public static MigrationInfo M2 = new MigrationInfo(typeof(Migration2));
        public static MigrationInfo M3 = new MigrationInfo(typeof(Migration3));
        public static MigrationInfo M4 = new MigrationInfo(typeof(Migration4));
        public static MigrationInfo M5 = new MigrationInfo(typeof(Migration5));
        public static List<MigrationInfo> AllMigrations = new[] {M1, M2, M3, M4, M5}.ToList();
    }

    public class VersionInfo {
        public string MigrationGroup { get; set; }
        public long Version { get; set; }
        public string Info { get; set; }
        public DateTime Applied { get; set; }
    }

    public class Migration1 : Migration {
        public override void Up() {
        }

        public override void Down() {
        }
    }
    public class Migration2 : Migration1 { }
    public class Migration3 : Migration1 { }
    public class Migration4 : Migration1 { }
    public class Migration5 : Migration1 { }
}
