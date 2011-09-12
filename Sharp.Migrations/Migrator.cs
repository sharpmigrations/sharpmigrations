using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using log4net;

namespace Sharp.Migrations {
	public class Migrator {

		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

		private readonly List<Migration> _migrationsToRun;

		private int _versionFrom;
		private int _versionTo;

		public int CurrentVersion { get; set; }
		
		public Migrator(List<Migration> migrationsToRun) {
			_migrationsToRun = migrationsToRun;
		}

		public void Migrate() {
			SetVersionInterval();
			MigrateAll();
		}

		private void MigrateAll() {
			Log.Info("Starting migrations");

			CurrentVersion = _versionFrom;

			for (int i = 0; i < _migrationsToRun.Count; i++) {
				Migration migration = _migrationsToRun[i];
				if (IsUp()) {
					migration.Up();
					CurrentVersion = migration.Version;
				}
				else {
					CurrentVersion = migration.Version;
					migration.Down();

				}
				Log.Info(String.Format(" -> [{0}] {1} {2}()", migration.Version, migration.GetType().Name, IsUp() ? "Up" : "Down"));
			}

			Log.Info("Done. Current version: " + CurrentVersion);
		}

		private void SetVersionInterval() {
			_versionFrom = _migrationsToRun.First().Version;
			_versionTo = _migrationsToRun.Last().Version;
		}

		private bool IsUp() {
			return _versionFrom < _versionTo;
		}
	}
}