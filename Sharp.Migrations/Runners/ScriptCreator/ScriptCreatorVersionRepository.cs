using System;
using Sharp.Data;

namespace Sharp.Migrations.Runners.ScriptCreator {
	public class ScriptCreatorVersionRepository : VersionRepository {

	    public event Action<long> OnUpdateVersion;

	    protected virtual void RaiseOnUpdateVersion(long version) {
	        Action<long> handler = OnUpdateVersion;
            if (handler != null) handler(version);
	    }

	    public ScriptCreatorVersionRepository(IDataClient dataClient) : base(dataClient) {}

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
            RaiseOnUpdateVersion(migrationInfo.Version);
	    }

	    public override void RemoveVersion(MigrationInfo migrationInfo) {
	        base.RemoveVersion(migrationInfo);
            RaiseOnUpdateVersion(migrationInfo.Version);
	    }
	}
}