using System.Configuration;

namespace Sharp.Data {
    public class SharpFactory : ISharpFactory {

        public string ConnectionString { get; set; }
        public string DatabaseProviderName { get; set; }

        public SharpFactory() {
            if (ConfigurationManager.ConnectionStrings.Count <= 0) return;
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[0];
            ConnectionString = settings.ConnectionString;
            DatabaseProviderName = settings.ProviderName;
        }

        public SharpFactory(string connectionString, string databaseProviderName) {
            ConnectionString = connectionString;
            DatabaseProviderName = databaseProviderName;
        }

        public IDataProvider CreateDataProvider(string databaseProviderName) {
            return GetConfig(databaseProviderName).DataProvider;
        }

        public IDataProvider CreateDataProvider() {
            return GetConfig(DatabaseProviderName).DataProvider;
        }

        public IDatabase CreateDatabase() {
            return CreateDatabaseInternal(GetConfig(DatabaseProviderName), ConnectionString);
        }

        public IDatabase CreateDatabase(string connectionString, string databaseProviderName) {
            return CreateDatabaseInternal(GetConfig(databaseProviderName), connectionString);
        }

        private IDatabase CreateDatabaseInternal(SharpDbConfig sharpDbConfig, string connectionString) {
            return new Database(sharpDbConfig.DataProvider, connectionString);
        }

        public IDataClient CreateDataClient(string connectionString, string databaseProviderName) {
            SharpDbConfig config = GetConfig(databaseProviderName);
            return new DataClient(CreateDatabaseInternal(config, connectionString), config.Dialect);
        }

        public IDataClient CreateDataClient() {
            SharpDbConfig config = GetConfig(DatabaseProviderName);
            return new DataClient(CreateDatabaseInternal(config, ConnectionString), config.Dialect);
        }

        private SharpDbConfig GetConfig(string databaseProviderName) {
            SharpDbProviderFactory factory = new SharpDbProviderFactory();
            return factory.CreateSharpDbConfig(databaseProviderName);
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
