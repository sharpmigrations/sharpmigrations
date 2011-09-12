using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Sharp.Data;
using Sharp.Migrations;
using Sharp.Data.Config;

namespace Sharp.Migrations {
    public class Runner {

		public static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

    	private Assembly _targetAssembly;
    	private IDataClient _dataClient;
    	private List<Migration> _migrationsToRun = new List<Migration>();

		private int _currentVersion, _initialVersion, _targetVersion, _maxVersion;

		public IVersionRepository VersionRepository { private get; set; }

		public Runner(IDataClient dataClient, Assembly targetAssembly) {
			_dataClient = dataClient;
			_targetAssembly = targetAssembly ?? Assembly.GetCallingAssembly();
			VersionRepository = new VersionRepository(_dataClient);
		}

        public void Run(int version) {
        	CreateVersionTable();
			GetCurrentVersion();
			RunMigrations(version);
        }

    	private void CreateVersionTable() {
			try {
				VersionRepository.CreateVersionTable();
				LogInfo("Schema_info table created");
			}
			catch {}
    	}

    	private void GetCurrentVersion() {
    		_initialVersion = VersionRepository.GetCurrentVersion();
    	}

    	private void RunMigrations(int version) {
    		_targetVersion = version;
    		CreateMigrationsToRun();
			RunCreatedMigraions();
    	}

    	private bool NoWorkToDo() {
    		return _migrationsToRun.Count == 0;
    	}

    	private void CreateMigrationsToRun() {
			List<Type> migrationTypes = GetMigrationTypes();
			
			MigrationFactory factory = new MigrationFactory(_dataClient);
			foreach (var type in migrationTypes) {
				Migration migration = factory.CreateMigration(type);
				_migrationsToRun.Add(migration);
			}
    	}

    	private List<Type> GetMigrationTypes() {
			MigrationFinder migrationFinder = new MigrationFinder(_targetAssembly);

    		_maxVersion = migrationFinder.LastVersion;
			if (_targetVersion < 0) _targetVersion = _maxVersion;

			return migrationFinder.FromVersion(_initialVersion)
											 .ToVersion(_targetVersion)
    										 .FindMigrations();
    	}

    	private void RunCreatedMigraions() {
			if (NoWorkToDo()) {
				LogInfo("No migrations to perform");
				return;
			}

			Log.Info("Starting migrations");
			Log.Info("Max version is " + _maxVersion);
			Log.Info("Migrate from " + _initialVersion + " to " + _targetVersion);

			_currentVersion = _initialVersion;
			try {
				for (int i = 0; i < _migrationsToRun.Count; i++) {
					RunOneMigration(i);
				}
				Log.Info("Done. Current version: " + _currentVersion);
			}
    		finally {
				VersionRepository.UpdateVersion(_currentVersion);	
			}
    	}

    	private void RunOneMigration(int i) {
    		Migration migration = _migrationsToRun[i];
    		if (IsUp()) {
    			migration.Up();
    			_currentVersion = migration.Version;
    		}
    		else {
    			migration.Down();
    			if(i < _migrationsToRun.Count-1) {
    				_currentVersion = _migrationsToRun[i + 1].Version;							
    			}
    			else {
    				_currentVersion = _targetVersion;
    			}
    		}
			Log.Info(String.Format(" -> [{0}] {1} {2}()", migration.Version, migration.GetType().Name, IsUp() ? "Up" : "Down"));
    	}


    	private bool IsUp() {
			return _initialVersion < _targetVersion;
		}

		private void LogInfo(string message) {
			if (Log.IsInfoEnabled) {
				Log.Info(message);
			}
		}
    }
}