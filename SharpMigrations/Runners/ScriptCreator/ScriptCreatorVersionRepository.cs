using System;
using SharpData;

namespace SharpMigrations.Runners.ScriptCreator {
	public class ScriptCreatorVersionRepository : VersionRepository {

	    public event Action<long> OnUpdateVersion;

	    public ScriptCreatorVersionRepository(IDataClient dataClient, string migrationGroup) : base(dataClient, migrationGroup) {}

        public override long GetCurrentVersion() {
            try {
                return base.GetCurrentVersion();
            }
            catch {
                return -1;
            }
        }

	    public override void InsertVersion(MigrationInfo migrationInfo) {
	        base.InsertVersion(migrationInfo);
	        OnUpdateVersion?.Invoke(migrationInfo.Version);
	    }

	    public override void RemoveVersion(MigrationInfo migrationInfo) {
	        base.RemoveVersion(migrationInfo);
	        OnUpdateVersion?.Invoke(migrationInfo.Version);
	    }
	}
}