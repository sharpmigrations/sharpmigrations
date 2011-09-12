using System;
using System.Reflection;
using Sharp.Data;
using Sharp.Data.Config;
using Sharp.Data.Filters;
using Sharp.Data.Schema;

namespace Sharp.Migrations {
	public class VersionRepository : IVersionRepository {

        public const string VERSION_TABLE_NAME = "schema_info";
        public const string DEFAULT_MIGRATION_GROUP = "default";

        private IDataClient _dataClient;
	    private Filter MigrationGroupFilter;
	    private string _migrationGroup;

	    public string MigrationGroup {
	        get { return _migrationGroup; }
	        set {
	            _migrationGroup = value;
                MigrationGroupFilter = Filter.Eq("name", MigrationGroup);
	        }
	    }

	    public VersionRepository(IDataClient dataClient) {
            _dataClient = dataClient;
	        _migrationGroup = DEFAULT_MIGRATION_GROUP;
            CreateVersionTable();
        }

	    private void CreateVersionTable() {
	        try {
	            TryCreateVersionTable();
	        }
	        catch(Exception ex) {
	            throw new MigrationException("Could not create schema version table. See inner exception for details.", ex);
	        }
	        finally {
	            _dataClient.Close();
	        }
	    }

	    private void TryCreateVersionTable() {
            if (_dataClient.TableExists(VERSION_TABLE_NAME)) {
                return;
            }
	        _dataClient.Add
	            .Table(VERSION_TABLE_NAME)
	            .WithColumns(
	                Column.String("name").Size(200),
	                Column.Int64("version")
	            );
	    }

	    public int GetCurrentVersion() {
            try {
                return TryGetCurrentVersion();
            }
            catch(Exception ex) {
                throw new MigrationException("Could not get schema version. See inner exception for details.", ex);
            }
            finally {
                _dataClient.Close();
            }
	    }

	    private int TryGetCurrentVersion() {
            if (!MigrationGroupExists()) {
                InsertInitialVersionValue();
                return 0;
            }
            object ret = _dataClient.Select.Columns("version")
	                                .From(VERSION_TABLE_NAME)
	                                .Where(MigrationGroupFilter).AllRows()[0][0];
	        int version = Convert.ToInt32(ret);
	        return version;
	    }
	    
	    private bool MigrationGroupExists() {
            int count = _dataClient.Count.Table(VERSION_TABLE_NAME).Where(MigrationGroupFilter);
            return count != 0;
        }

	    private void InsertInitialVersionValue() {
			_dataClient.Insert
				.Into(VERSION_TABLE_NAME)
				.Columns("version","name")
                .Values(0, MigrationGroup);

			_dataClient.Commit();
		}

	    public void UpdateVersion(int version) {
            try {
            	_dataClient.Update.Table(VERSION_TABLE_NAME)
            					  .SetColumns("version")
            					  .ToValues(version)
                                  .Where(MigrationGroupFilter);
                _dataClient.Commit();
            }
            finally {
                _dataClient.Close();
            }
        }
	}
}