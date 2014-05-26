using System.Collections.Generic;
using Sharp.Migrations.Runners;

namespace Sharp.Migrations {
	public interface IVersionRepository {
        string MigrationGroup { get; set; }
		long GetCurrentVersion();
        void EnsureSchemaVersionTable(List<MigrationInfo> allMigrationsFromAssembly);
	    List<long> GetAppliedMigrations();
        void InsertVersion(MigrationInfo migrationInfo);
        void RemoveVersion(MigrationInfo migrationInfo);
    }
}