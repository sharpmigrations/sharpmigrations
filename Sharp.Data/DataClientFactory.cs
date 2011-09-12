using System;
using System.Configuration;
using System.Data.Common;
using Sharp.Data.Databases;
using Sharp.Data.Databases.MySql;
using Sharp.Data.Dialects;
using Sharp.Data.Providers;

namespace Sharp.Data {
    
    public class DataClientFactory : IDataClientFactory {

        public string DefaultConnectionString { get; set; }
        public string DefaultDataProvider { get; set; }

        public DataClientFactory() {}

        public DataClientFactory(string connectionString, string dataProviderName) {
            DefaultConnectionString = connectionString;
            DefaultDataProvider = dataProviderName;
        }


        public IDataClient GetDefaultDataClient() {
            CheckDefaultValues();
            return GetDataClient(DefaultConnectionString, DefaultDataProvider);
        }

        private void CheckDefaultValues() {
            if (String.IsNullOrEmpty(DefaultConnectionString)) {
                throw new ConfigurationErrorsException("This.DefaultConnectionString not set.");
            }
            if (String.IsNullOrEmpty(DefaultDataProvider)) {
                throw new ConfigurationErrorsException("This.DefaultProvider not set.");
            }
        }

        public IDataClient GetDataClient(string connectionString, string databaseProviderName) {
            IDataProvider dataProvider;
        	Dialect dialect;

            DbProviderFactory _dbProviderFactory = GetDbProviderFactory(databaseProviderName);

            if (databaseProviderName == DataProviderNames.OracleOdp) {
                dataProvider = new OracleOdpProvider(_dbProviderFactory);
                dialect = new OracleDialect();
            }
            else if (databaseProviderName == DataProviderNames.OracleClient) {
                dataProvider = new OracleClientProvider(_dbProviderFactory);
                dialect = new OracleDialect();
            }
            else if (databaseProviderName == DataProviderNames.SqlServer) {
                dataProvider = new SqlProvider(_dbProviderFactory);
                dialect = new SqlDialect();
            }
            else if (databaseProviderName == DataProviderNames.SqLite) {
                dataProvider = new SqLiteProvider(_dbProviderFactory);
                dialect = new SqLiteDialect();
            }
            else if (databaseProviderName == DataProviderNames.MySql) {
                dataProvider = new MySqlProvider(_dbProviderFactory);
                dialect = new MySqlDialect();
            }
            else {
                throw new ProviderNotFoundException("Could not find provider " + databaseProviderName);
            }
            return new DataClient(new Database(dataProvider, connectionString), dialect);
        }

        private DbProviderFactory GetDbProviderFactory(string databaseProviderName) {
            try {
                return DbProviderFactories.GetFactory(databaseProviderName);
            }
            catch (Exception ex) {
                throw new DataClientFactoryNotFoundException(databaseProviderName, ex);
            }
        }
    }
}