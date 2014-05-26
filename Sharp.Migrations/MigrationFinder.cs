using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sharp.Migrations.Runners;

namespace Sharp.Migrations {
	public class MigrationFinder {
		private Assembly _assembly;
		private List<Type> _allMigrations;
		private int _fromVersion;
		private int _toVersion;
		
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
                                      .OrderBy(VersionHelper.GetVersion)
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
            var selectedMigrations = new List<Type>(_allMigrations);
			if(IsDownMigration()) {
				InvertFromAndTo();
			    selectedMigrations.Reverse();
			}
            return selectedMigrations.Where(p => VersionHelper.GetVersion(p) > _fromVersion &&
											     VersionHelper.GetVersion(p) <= _toVersion).ToList();
		}

		private void InvertFromAndTo() {
			int aux = _fromVersion;
			_fromVersion = _toVersion;
			_toVersion = aux;
		}

		private bool IsDownMigration() {
			return _fromVersion > _toVersion;
		}

	    public static List<MigrationInfo> FindMigrations(Assembly assembly) {
            return assembly.GetTypes()
                           .Where(p => p.IsSubclassOf(typeof(Migration)) && !p.IsAbstract)
                           .Select(x => new MigrationInfo(x))
                           .OrderBy(x => x.Version)
                           .ToList();
	    } 
	}
}