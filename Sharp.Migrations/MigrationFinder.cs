using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sharp.Migrations.Runners;

namespace Sharp.Migrations {
	public static class MigrationFinder {
	    public static List<MigrationInfo> FindMigrations(Assembly assembly) {
            return assembly.GetTypes()
                           .Where(p => p.IsSubclassOf(typeof(Migration)) && !p.IsAbstract)
                           .Select(x => new MigrationInfo(x))
                           .OrderBy(x => x.Version)
                           .ToList();
	    }

        public static long FindLastMigration(Assembly assembly) {
            return FindMigrations(assembly).Last().Version;
        }

	    public static Type FindSeed(Assembly assembly, string seedName) {
	        var type =  assembly.GetTypes()
                                .FirstOrDefault(p => p.Name.ToUpper() == seedName.ToUpper() &&
                                                     p.IsSubclassOf(typeof (SeedMigration)) && 
                                                     !p.IsAbstract);
	        if (type == null) {
	            throw new SeedNotFoundException(seedName, "Could not find seed named " + seedName);
	        }
	        return type;
	    }
	}
}