using System.Reflection;
using log4net;
using Sharp.Data;

namespace Sharp.Migrations {
    public abstract class Migration {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        private IDataClient _dataClient;

        public int Version { get; set; }
        public string Name { get; protected set; }

        protected IDataClient DataClient {
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