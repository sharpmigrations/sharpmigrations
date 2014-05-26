using System;
using System.Text.RegularExpressions;

namespace Sharp.Migrations {
    
	public static class VersionHelper {

        public static int GetVersion(Type type) {
			string versionInString = Regex.Match(type.Name, "[0-9]+").Value;
			int version = ParseVersion(type, versionInString);
			return version;
        }

		private static int ParseVersion(Type type, string versionInString) {
			int version;
			if(!Int32.TryParse(versionInString, out version)) {
				throw new InvalidMigrationException(String.Format("Could not figure out the version of migration {0}", type.Name));
			}
			return version;
		}
	}
}