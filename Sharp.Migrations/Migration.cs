using Sharp.Data;

namespace Sharp.Migrations {
    public abstract class Migration {
    	private IDataClient _dataClient;
        public long Version { get; set; }
        public string Name { get; protected set; }

        public IDataClient DataClient {
            get { return _dataClient; }
        }

    	protected Migration() {
            Version = VersionHelper.GetVersion(GetType());
            Name = GetType().Name;
        }

        public void SetDataClient(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        public abstract void Up();
        public abstract void Down();
    }
}