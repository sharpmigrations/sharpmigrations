using System;
using System.Text.RegularExpressions;

namespace SharpMigrations {
    
	public static class VersionHelper {

        public static long GetVersion(Type type) {
			var versionInString = Regex.Match(type.Name, "[0-9]+").Value;
            return ParseVersion(type, versionInString);
        }

        private static long ParseVersion(Type type, string versionInString) {
            if (!Int64.TryParse(versionInString, out long version)) {
                throw new InvalidMigrationException(String.Format("Could not figure out the version of migration {0}", type.Name));
            }
            return version;
		}
	}
}