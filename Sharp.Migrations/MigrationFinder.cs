using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sharp.Migrations {
	public class MigrationFinder {
		private readonly Assembly _assembly;
		private List<Type> _allMigrations;
		private int _fromVersion;
		private int _toVersion;
		private bool _revert;
		
		public MigrationFinder(Assembly assembly) {
			_assembly = assembly;
		}

		public int LastVersion { 
			get {
				FindAllMigrations();
				return (_allMigrations.Count > 0) ? VersionHelper.GetVersion(FindAllMigrations().Last()) : 0;
			}
		}

		public List<Type> FindAllMigrations() {
			if(_allMigrations == null) {
                LoadAllMigrations();	
			}
			return _allMigrations;
		}

        private void LoadAllMigrations() {
            _allMigrations = _assembly.GetTypes()
                                                  .Where(p => p.IsSubclassOf(typeof(Migration)) && !p.IsAbstract)
                                                  .OrderBy(p => VersionHelper.GetVersion(p))
                                                  .ToList();
        }

		public MigrationFinder FromVersion(int version) {
			_fromVersion = version;
			return this;
		}

		public MigrationFinder ToVersion(int toVersion) {
			_toVersion = toVersion;
			return this;
		}

		public List<Type> FindMigrations() {

			FindAllMigrations();

			if(IsDownMigration()) {
				InvertFromAndTo();
			}

			List<Type> types = _allMigrations.Where(p => VersionHelper.GetVersion(p) > _fromVersion &&
													     VersionHelper.GetVersion(p) <= _toVersion).ToList();

			if (_revert) types.Reverse();
			return types;
		}

		private void InvertFromAndTo() {
			int aux = _fromVersion;
			_fromVersion = _toVersion;
			_toVersion = aux;
			_revert = true;
		}

		private bool IsDownMigration() {
			return _fromVersion > _toVersion;
		}
	}
}