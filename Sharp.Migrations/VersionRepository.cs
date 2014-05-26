using System;
using System.Collections.Generic;
using System.Linq;
using Sharp.Data;
using Sharp.Data.Filters;
using Sharp.Data.Schema;
using Sharp.Migrations.Runners;

namespace Sharp.Migrations {
    public class VersionRepository : IVersionRepository {

        public const string VERSION_TABLE_NAME = "sharpmigrations";
        public const string OLD_VERSION_TABLE_NAME = "schema_info";
        public const string DEFAULT_MIGRATION_GROUP = "default";

        private IDataClient _dataClient;
        private Filter _migrationGroupFilter;
        private string _migrationGroup;

        public string MigrationGroup {
            get { return _migrationGroup; }
            set {
                _migrationGroup = value;
                _migrationGroupFilter = Filter.Eq("migrationgroup", MigrationGroup);
            }
        }

        public VersionRepository(IDataClient dataClient) {
            _dataClient = dataClient;
            MigrationGroup = DEFAULT_MIGRATION_GROUP;
        }

        public void EnsureSchemaVersionTable(List<MigrationInfo> allMigrationsFromAssembly) {
            if (CreateVersionTable()) {
                MigrateOldSchema(allMigrationsFromAssembly);
            }
        }

        private bool CreateVersionTable() {
            if (_dataClient.TableExists(VERSION_TABLE_NAME)) {
                return false;
            }
            try {
                TryCreateVersionTable();
                return true;
            }
            catch (Exception ex) {
                throw new MigrationException("Could not create schema version table (" + VERSION_TABLE_NAME + "). See inner exception for details.", ex);
            }
            finally {
                _dataClient.Close();
            }
        }

        private void TryCreateVersionTable() {
            _dataClient.Add
                .Table(VERSION_TABLE_NAME)
                .WithColumns(
                    Column.String("migrationgroup").Size(200),
                    Column.Int64("version").NotNull(),
                    Column.String("info").Size(1000),
                    Column.Date("applied")
                );
            _dataClient.AddPrimaryKey(VERSION_TABLE_NAME, "migrationgroup", "version");
            _dataClient.Commit();
        }

        private void MigrateOldSchema(List<MigrationInfo> allMigrationsFromAssembly) {
            if (!_dataClient.TableExists(OLD_VERSION_TABLE_NAME)) {
                return;
            }
            var res = _dataClient.Select.Columns("name", "version")
                .From(OLD_VERSION_TABLE_NAME)
                .Where(Filter.Eq("name", MigrationGroup))
                .AllRows();
            if (res.Count == 0) {
                return;
            }
            var version = Convert.ToInt32(res[0]["version"]);
            var migrationsToSave = allMigrationsFromAssembly.Where(m => m.Version <= version).OrderBy(m => m.Version);
            foreach (var migrationInfo in migrationsToSave) {
                InsertVersion(migrationInfo);
            }
            _dataClient.RemoveTable(OLD_VERSION_TABLE_NAME);
        }

        public List<long> GetAppliedMigrations() {
            try {
                return TryGetAppliedMigrations();
            }
            catch (Exception ex) {
                throw new MigrationException("Could not retrieve applied (table " + VERSION_TABLE_NAME + "). See inner exception for details.", ex);
            }
            finally {
                _dataClient.Close();
            }
        }

        public virtual void InsertVersion(MigrationInfo migrationInfo) {
            _dataClient.Insert
                .Into(VERSION_TABLE_NAME)
                .Columns("migrationgroup", "version", "info", "applied")
                .Values(MigrationGroup, migrationInfo.Version, migrationInfo.Name, DateTime.Now);
            _dataClient.Commit();
        }

        public virtual void RemoveVersion(MigrationInfo migrationInfo) {
            var filterGroup = Filter.Eq("migrationgroup", MigrationGroup);
            var filterVersion = Filter.Eq("version", migrationInfo.Version);
            _dataClient.Delete
                       .From(VERSION_TABLE_NAME)
                       .Where(Filter.And(filterGroup, filterVersion));
            _dataClient.Commit();
        }

        private List<long> TryGetAppliedMigrations() {
            return _dataClient.Select.Columns("version")
                .From(VERSION_TABLE_NAME)
                .Where(_migrationGroupFilter)
                .OrderBy(OrderBy.Ascending("version"))
                .AllRows()
                .Select(x => Convert.ToInt64(x["version"]))
                .ToList();
        }

        public virtual long GetCurrentVersion() {
            try {
                return TryGetCurrentVersion();
            }
            catch (Exception ex) {
                throw new MigrationException("Could not get schema version (table "+VERSION_TABLE_NAME+"). See inner exception for details.", ex);
            }
            finally {
                _dataClient.Close();
            }
        }

        private long TryGetCurrentVersion() {
            if (!_dataClient.TableExists(VERSION_TABLE_NAME)) {
                if (_dataClient.TableExists(OLD_VERSION_TABLE_NAME)) {
                    return
                        Convert.ToInt64(_dataClient.Select.Columns("version")
                            .From(OLD_VERSION_TABLE_NAME)
                            .Where(Filter.Eq("name", MigrationGroup))
                            .AllRows()[0][0]);
                }
                return 0;
            }
            if (!MigrationGroupExists()) {
                return 0;
            }
            var sql = "select max(version) from " + VERSION_TABLE_NAME + " where migrationgroup = '" + MigrationGroup + "'";
            var version = Convert.ToInt64(_dataClient.Database.QueryScalar(sql));
            return version;
        }

        private bool MigrationGroupExists() {
            int count = _dataClient.Count.Table(VERSION_TABLE_NAME).Where(_migrationGroupFilter);
            return count != 0;
        }
    }
}