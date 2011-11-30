using System.Configuration;

namespace Sharp.Data {
    public class SharpFactory : ISharpFactory {

        public string ConnectionString { get; set; }
        public string DataProviderName { get; set; }

        public SharpFactory() {
            if (ConfigurationManager.ConnectionStrings.Count <= 0) return;
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[0];
            ConnectionString = settings.ConnectionString;
            DataProviderName = settings.ProviderName;
        }

        public SharpFactory(string connectionString, string databaseProviderName) {
            ConnectionString = connectionString;
            DataProviderName = databaseProviderName;
        }

        public IDataProvider CreateDataProvider(string databaseProviderName) {
			return GetConfig().DataProvider;
        }

        public IDataProvider CreateDataProvider() {
            return GetConfig().DataProvider;
        }

        public IDatabase CreateDatabase() {
        	return GetConfig().Database;
        }

        public IDatabase CreateDatabase(string connectionString, string databaseProviderName) {
        	return GetConfig(databaseProviderName, connectionString).Database;
        }

        public IDataClient CreateDataClient(string connectionString, string databaseProviderName) {
			return GetConfig(databaseProviderName, connectionString).DataClient;            
        }

        public IDataClient CreateDataClient() {
			return GetConfig().DataClient;                        
        }

		private SharpDbConfig GetConfig() {
			return GetConfig(DataProviderName, ConnectionString);
		}

		private SharpDbConfig GetConfig(string databaseProviderName, string connectionString) {
            SharpDbProviderFactory factory = new SharpDbProviderFactory();
            return factory.CreateSharpDbConfig(databaseProviderName, connectionString);
        }

        private static ISharpFactory _default;
        public static ISharpFactory Default {
            get {
                if (_default == null) {
                    _default = new SharpFactory();
                }
                return _default;
            }
            set { _default = value; }
        }
    }
}
