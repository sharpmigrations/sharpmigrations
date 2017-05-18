using System;
using System.Reflection;
using SharpData;
using SharpData.Databases;
using SharpData.Log;

namespace SharpMigrations.Runners {
    public class Runner : IRunner {
        public static ISharpLogger Log = LogManager.GetLogger(typeof(Runner).Name);
        public static bool IgnoreDialectNotSupportedActions { get; set; }

        private long _initialVersion;
        private Assembly _targetAssembly;
        private IDataClient _dataClient;
        private DatabaseKind _databaseKind;
        private IVersionRepository _versionRepository;
        private MigrationFactory _migrationFactory;

        public event EventHandler<MigrationErrorArgs> OnMigrationError;

        private void FireOnMigrationError(MigrationErrorArgs args) {
            OnMigrationError?.Invoke(this, args);
        }
        
	    public long LastVersionNumber => MigrationFinder.FindLastMigration(_targetAssembly);

        public long CurrentVersionNumber {
	        get {
	            if (_initialVersion == -1) {
                    GetCurrentVersion();
	            }
	            return _initialVersion;
	        }
	    }

        public Runner(IDataClient dataClient, Assembly targetAssembly, string migrationGroup) 
            : this(dataClient, targetAssembly, new VersionRepository(dataClient, migrationGroup)) { }

        public Runner(IDataClient dataClient, 
                      Assembly targetAssembly, 
                      IVersionRepository versionRepository) {
            _dataClient = dataClient;
            _databaseKind = _dataClient.Database.Provider.DatabaseKind;
            _targetAssembly = targetAssembly;
            _versionRepository = versionRepository;
            _migrationFactory = new MigrationFactory(_dataClient);
            _initialVersion = -1;
	    }

		public void Run(long targetVersion) {
		    var migrationsFromAssembly = MigrationFinder.FindMigrations(_targetAssembly);
		    var migrationsFromDatabase = _versionRepository.GetAppliedMigrations();
		    var migrationPlan = new MigrationPlan(migrationsFromDatabase, migrationsFromAssembly, targetVersion);
            RunMigrations(migrationPlan);
		}

	    protected virtual void GetCurrentVersion() {
			_initialVersion = _versionRepository.GetCurrentVersion();
		}

	    private void RunMigrations(MigrationPlan migrationPlan) {
	        if (NoWorkToDo(migrationPlan)) {
	            Log.Info("No migrations to perform");
	            return;
	        }
	        Log.Info("Starting migrations");
	        Log.Info("Current version is " + migrationPlan.CurrentVersion);
	        Log.Info("Target version is " + migrationPlan.TargetVersion);
	        Log.Info(String.Format("Migrate action is: {0} from {1} to {2}",
	            (migrationPlan.IsUp ? "UP" : "DOWN"),
	            migrationPlan.CurrentVersion,
	            migrationPlan.TargetVersion));

	        foreach (var step in migrationPlan.OrderedSteps) {
	            var migrationInfo = step.MigrationInfo;
	            try {
	                RunMigration(step);
	            }
	            catch (NotSupportedByDialect nse) {
	                HandleNotSupportedByDialectException(migrationInfo, nse);
	            }
	            catch (Exception ex) {
	                var errorMsg = String.Format("Error running migration {0}: {1}", migrationInfo.Name, ex);
                    Log.Error(errorMsg);
	                _dataClient.RollBack();
                    var args = new MigrationErrorArgs(migrationInfo.Name, ex);
                    FireOnMigrationError(args);
                    if (!args.Handled) {
                        throw new MigrationException(errorMsg, ex);
                    }
	            }
	        }
            Log.Info("Done. Current version: " + migrationPlan.TargetVersion);
	    }

        private void RunMigration(MigrationPlanStep step) {
            var migrationInfo = step.MigrationInfo;
            if (!migrationInfo.MigratesFor(_databaseKind)) {
                Log.Info(
                    $" -> [{migrationInfo.Version}] {migrationInfo.Name} {step.Direction}() NOT PERFORMED for database {_databaseKind}");
                UpdateCurrentVersion(step);
                return;
            }
            Log.Info(String.Format(" -> [{0}] {1} {2}()", migrationInfo.Version, migrationInfo.Name, step.Direction));
            
            var migration = _migrationFactory.CreateMigration(migrationInfo.MigrationType);
            if (step.Direction == Direction.Up) {
                migration.Up();
            }
            else {
                migration.Down();
            }
            UpdateCurrentVersion(step);
        }

        private void UpdateCurrentVersion(MigrationPlanStep step) {
            if (!step.ShouldUpdateVersion) {
                return;
            }
            if (step.Direction == Direction.Up) {
                _versionRepository.InsertVersion(step.MigrationInfo);
	        }
	        else {
                _versionRepository.RemoveVersion(step.MigrationInfo);
	        }
		}

		private bool NoWorkToDo(MigrationPlan migrationPlan) {
			return migrationPlan.OrderedSteps.Count == 0;
		}
        
		private void HandleNotSupportedByDialectException(MigrationInfo migrationInfo, NotSupportedByDialect nse) {
		    if (!IgnoreDialectNotSupportedActions) {
		        throw nse;
		    }
		    Log.Warn(
		        $"Migration[{migrationInfo.Name}] NotSupportedException not thrown due user config. Dialect: {nse.DialectName} Function: {nse.FunctionName} Msg: {nse.Message}");
		}
	}
}