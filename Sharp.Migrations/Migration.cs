using Sharp.Data;

namespace Sharp.Migrations {
    public abstract class Migration {
    	private IDataClient _dataClient;
        public long Version { get; set; }

        public IDataClient DataClient {
            get { return _dataClient; }
        }

    	protected Migration() {
            Version = VersionHelper.GetVersion(GetType());
        }

        public void SetDataClient(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        public void ExecuteSql(string call, params object[] parameters) {
            _dataClient.Database.ExecuteSql(call, parameters);
        }

        public abstract void Up();
        public abstract void Down();
    }
}