using System.Collections.Generic;
using SharpMigrations.Runners;

namespace SharpMigrations {
	public interface IVersionRepository {
        string MigrationGroup { get; }
		long GetCurrentVersion();
	    List<long> GetAppliedMigrations();
        void InsertVersion(MigrationInfo migrationInfo);
        void RemoveVersion(MigrationInfo migrationInfo);
    }
}