using System;
using Sharp.Data;

namespace Sharp.Migrations.Runners.ScriptCreator {
	public class ScriptCreatorVersionRepository : VersionRepository {

	    public event Action<int> OnUpdateVersion;

	    protected virtual void RaiseOnUpdateVersion(int version) {
	        Action<int> handler = OnUpdateVersion;
            if (handler != null) handler(version);
	    }

	    public ScriptCreatorVersionRepository(IDataClient dataClient, bool createVersionTable = true) : base(dataClient, createVersionTable) {}

        public override int GetCurrentVersion() {
            try {
                return base.GetCurrentVersion();
            }
            catch {
                CreateVersionTable();
                InsertInitialVersionValue();
                return 0;
            }
        }

        public override void UpdateVersion(int version) {
            base.UpdateVersion(version);
            RaiseOnUpdateVersion(version);
        }
	}
}