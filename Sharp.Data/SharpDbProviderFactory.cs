using System;
using System.Data.Common;
using Sharp.Data.Databases;
using Sharp.Data.Databases.MySql;
using Sharp.Data.Databases.Oracle;
using Sharp.Data.Databases.SqLite;
using Sharp.Data.Databases.SqlServer;
using Sharp.Data.Providers;

namespace Sharp.Data {
    public class SharpDbProviderFactory {
        public SharpDbConfig CreateSharpDbConfig(string databaseProviderName, string connectionString) {

            DbProviderFactory dbProviderFactory = GetDbProviderFactory(databaseProviderName);

            IDataProvider dataProvider;
        	Database database;
            DataClient dataClient;

            if (databaseProviderName == DataProviderNames.OracleManaged) {
                dataProvider = new OracleManagedProvider(dbProviderFactory);
				database = new Database(dataProvider, connectionString);
				dataClient = new OracleDataClient(database);
            }
            else if (databaseProviderName == DataProviderNames.OracleOdp) {
                dataProvider = new OracleOdpProvider(dbProviderFactory);
                database = new Database(dataProvider, connectionString);
                dataClient = new OracleDataClient(database);
            }
            else if (databaseProviderName == DataProviderNames.SqlServer) {
                dataProvider = new SqlProvider(dbProviderFactory);
				database = new Database(dataProvider, connectionString);
				dataClient = new SqlServerDataClient(database);
            }
            else if (databaseProviderName == DataProviderNames.OleDb) {
                dataProvider = new SqlProvider(dbProviderFactory);
                database = new Database(dataProvider, connectionString);
                dataClient = new SqlServerDataClient(database);
            }
            else if (databaseProviderName == DataProviderNames.SqLite) {
                dataProvider = new SqLiteProvider(dbProviderFactory);
				database = new Database(dataProvider, connectionString);
				dataClient = new SqLiteDataClient(database);
            }
            else if (databaseProviderName == DataProviderNames.MySql) {
                dataProvider = new MySqlProvider(dbProviderFactory);
				database = new Database(dataProvider, connectionString);
				dataClient = new MySqlDataClient(database);
            }
            else {
                throw new ProviderNotFoundException("Could not find provider " + databaseProviderName);
            }
            var config = new SharpDbConfig {
                DbProviderName = databaseProviderName,
                DataProvider = dataProvider,
				Database = database,
        		DataClient = dataClient
            };
            return config;
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
