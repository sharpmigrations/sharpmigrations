using System;
using System.Data.Common;

namespace Sharp.Data {
    public abstract class DbFactory {
        public string ConnectionString { get; set; }
        public string DatabaseProviderName { get; set; }

        protected DbFactory(string databaseProviderName, string connectionString) {
            ConnectionString = connectionString;
            DatabaseProviderName = databaseProviderName;
        }

        public abstract IDataProvider CreateDataProvider();
        public virtual IDatabase CreateDatabase() {
            return new Database(CreateDataProvider(), ConnectionString);
        }
        public abstract Dialect CreateDialect();
        public abstract IDataClient CreateDataClient();

        protected DbProviderFactory GetDbProviderFactory(string databaseProviderName) {
            try {
                return DbProviderFactories.GetFactory(databaseProviderName);
            }
            catch (Exception ex) {
                throw new DataClientFactoryNotFoundException(databaseProviderName, ex);
            }
        }
    }
}