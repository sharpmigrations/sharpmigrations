using System;
using System.Collections.Generic;
using System.Linq;
using SharpData;
using SharpData.Filters;
using SharpData.Schema;
using SharpMigrations.Runners;

namespace SharpMigrations {
    public class VersionRepository : IVersionRepository {

        public const string VERSION_TABLE_NAME = "sharpmigrations";
        public const string DEFAULT_MIGRATION_GROUP = "default";

        private IDataClient _dataClient;
        private Filter _migrationGroupFilter;
        public string MigrationGroup { get; }

        public VersionRepository(IDataClient dataClient, string migrationGroup = DEFAULT_MIGRATION_GROUP) {
            _dataClient = dataClient;
            MigrationGroup = migrationGroup ?? DEFAULT_MIGRATION_GROUP;
            _migrationGroupFilter = Filter.Eq("migrationgroup", MigrationGroup);
            EnsureSchemaVersionTable();
        }

        public void EnsureSchemaVersionTable() {
            CreateVersionTable();
        }

        private void CreateVersionTable() {
            if (_dataClient.TableExists(VERSION_TABLE_NAME)) {
                return;
            }
            try {
                TryCreateVersionTable();
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
                    Column.String("migrationgroup").Size(200).NotNull(),
                    Column.Int64("version").NotNull(),
                    Column.String("info").Size(1000),
                    Column.Date("applied")
                );
            _dataClient.AddPrimaryKey(VERSION_TABLE_NAME, "migrationgroup", "version");
            _dataClient.Commit();
        }

        public List<long> GetAppliedMigrations() {
            try {
                return TryGetAppliedMigrations();
            }
            catch (Exception ex) {
                throw new MigrationException("Could not retrieve schema version table (" + VERSION_TABLE_NAME + "). See inner exception for details.", ex);
            }
            finally {
                _dataClient.Close();
            }
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

        public virtual void InsertVersion(MigrationInfo migrationInfo) {
            _dataClient.Insert
                .Into(VERSION_TABLE_NAME)
                .Columns("migrationgroup", "version", "info", "applied")
                .Values(MigrationGroup, migrationInfo.Version, migrationInfo.Name, DateTime.Now);
            _dataClient.Commit();
        }

        public virtual void RemoveVersion(MigrationInfo migrationInfo) {
            var filterVersion = Filter.Eq("version", migrationInfo.Version);
            _dataClient.Delete
                       .From(VERSION_TABLE_NAME)
                       .Where(Filter.And(_migrationGroupFilter, filterVersion));
            _dataClient.Commit();
        }

        public virtual long GetCurrentVersion() {
            try {
                return TryGetCurrentVersion();
            }
            catch (Exception ex) {
                throw new MigrationException("Could not get schema version (table " + VERSION_TABLE_NAME + "). See inner exception for details.", ex);
            }
            finally {
                _dataClient.Close();
            }
        }

        private long TryGetCurrentVersion() {
            if (!_dataClient.TableExists(VERSION_TABLE_NAME) || !MigrationGroupExists()) {
                return 0;
            }
            var sql = "select max(version) from " + VERSION_TABLE_NAME + " where migrationgroup = '" + MigrationGroup + "'";
            return Convert.ToInt64(_dataClient.Database.QueryScalar(sql));
        }

        private bool MigrationGroupExists() {
            return _dataClient.Count
                              .Table(VERSION_TABLE_NAME)
                              .Where(_migrationGroupFilter) > 0;
        }
    }
}