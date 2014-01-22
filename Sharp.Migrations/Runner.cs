using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Data.Log;
using Sharp.Migrations.Attributes;

namespace Sharp.Migrations {
	public class Runner {
        public static ISharpLogger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        public static bool IgnoreDialectNotSupportedActions { get; set; }

		private Assembly _targetAssembly;
		private IDataClient _dataClient;
	    private DatabaseKind _databaseKind;
		private int _currentVersion, _initialVersion, _targetVersion, _maxVersion;
        private MigrationFinder _migrationFinder;

        protected List<Migration> MigrationsToRun;
        public IVersionRepository VersionRepository { private get; set; }

	    public string MigrationGroup {
	        get { return VersionRepository.MigrationGroup; }
            set { VersionRepository.MigrationGroup = value; }
	    }

	    public int LastVersionNumber {
            get { return _migrationFinder.LastVersion; }
	    }

	    public int CurrentVersionNumber {
	        get {
	            if (_initialVersion == -1) {
                    GetCurrentVersion();
	            }
	            return _initialVersion;
	        }
	    }

        public Runner(IDataClient dataClient, Assembly targetAssembly, IVersionRepository versionRepository) {
            _dataClient = dataClient;
            _databaseKind = _dataClient.Database.Provider.DatabaseKind;
            _targetAssembly = targetAssembly ?? Assembly.GetCallingAssembly();
            VersionRepository = versionRepository;
            _migrationFinder = new MigrationFinder(_targetAssembly);
            _initialVersion = -1;
	    }
        
	    public Runner(IDataClient dataClient, Assembly targetAssembly) : this(dataClient, targetAssembly, new VersionRepository(dataClient)) {}

		public void Run(int version) {
			GetCurrentVersion();
			RunMigrations(version);
		}

		protected virtual void GetCurrentVersion() {
			_initialVersion = VersionRepository.GetCurrentVersion();
		}

		private void RunMigrations(int version) {
			_targetVersion = version;
			CreateMigrationsToRun();
			RunCreatedMigrations();
		}

		private void CreateMigrationsToRun() {
			List<Type> migrationTypes = GetMigrationTypes();
            MigrationsToRun = new List<Migration>();
			var factory = new MigrationFactory(_dataClient);
			foreach (Type type in migrationTypes) {
				Migration migration = factory.CreateMigration(type);
				MigrationsToRun.Add(migration);
			}
		}
       
		private List<Type> GetMigrationTypes() {
			_maxVersion = _migrationFinder.LastVersion;
			if (_targetVersion < 0) {
				_targetVersion = _maxVersion;
			}
			return _migrationFinder.FromVersion(_initialVersion)
				                   .ToVersion(_targetVersion)
				                   .FindMigrations();
		}

		private void RunCreatedMigrations() {
			if (NoWorkToDo()) {
				Log.Info("No migrations to perform");
				return;
			}
			Log.Info("Starting migrations");
			Log.Info("Max version is " + _maxVersion);
			Log.Info("Migrate from " + _initialVersion + " to " + _targetVersion);

			_currentVersion = _initialVersion;

			int i;
			for (i = 0; i < MigrationsToRun.Count; i++) {
				try {
					RunOneMigration(i);
				}
				catch (NotSupportedByDialect nse) {
					HandleNotSupportedByDialectException(i, nse);
				}
				catch (Exception ex) {
					string errorMsg = String.Format("Error running migration {0}: {1}", MigrationsToRun[i], ex); 
					Log.Error(errorMsg);
					_dataClient.RollBack();
					throw new MigrationException(errorMsg, ex);
				}
				finally {
					UpdateSchemaVersion(_currentVersion);
				}
			}
			Log.Info("Done. Current version: " + _currentVersion);
		}

        protected void UpdateSchemaVersion(int version) {
            VersionRepository.UpdateVersion(_currentVersion);
        }

		private bool NoWorkToDo() {
			return MigrationsToRun.Count == 0;
		}

		private void RunOneMigration(int i) {
			Migration migration = MigrationsToRun[i];
		    if (!ShouldMigrateForThisDatabase(migration)) {
                Log.Info(String.Format(" -> [{0}] {1} {2}() NOT PERFORMED for database {3}", migration.Version, migration.GetType().Name, IsUp() ? "Up" : "Down", _databaseKind));
		        SetCurrentVersion(i);
                return;
		    }
            if (IsUp()) {
                migration.Up();
			}
			else {
                migration.Down();
            }
		    Log.Info(String.Format(" -> [{0}] {1} {2}()", migration.Version, migration.GetType().Name, IsUp() ? "Up" : "Down"));
		    SetCurrentVersion(i);
		}

        private void SetCurrentVersion(int i) {
            if (IsUp()) {
                _currentVersion = MigrationsToRun[i].Version;
                return;
            }
	        if (IsNotTheLastMigration(i)) {
	            _currentVersion = MigrationsToRun[i + 1].Version;
                return;
	        }
            _currentVersion = _targetVersion;
	    }

	    private bool ShouldMigrateForThisDatabase(Migration migration) {
            Attribute[] attrs = Attribute.GetCustomAttributes(migration.GetType());
            if (attrs.Length == 0) return true;
            var onlyFor = (OnlyForAttribute) attrs[0];
            return onlyFor.DatabaseKinds.Contains(_databaseKind);
        }

		private bool IsNotTheLastMigration(int i) {
			return i < MigrationsToRun.Count - 1;
		}

		private void HandleNotSupportedByDialectException(int i, NotSupportedByDialect nse) {
			if (IgnoreDialectNotSupportedActions) {
				Log.Warn(
					String.Format(
						"Migration[{0}] NotSupportedException not thrown due user config. Dialect: {1} Function: {2} Msg: {3}",
						MigrationsToRun[i], nse.DialectName, nse.FunctionName, nse.Message));
				return;
			}
			throw nse;
		}

		private bool IsUp() {
			return _initialVersion < _targetVersion;
		}
	}
}