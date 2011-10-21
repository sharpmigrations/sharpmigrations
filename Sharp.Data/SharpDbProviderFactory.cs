using System;
using System.Data.Common;
using Sharp.Data.Databases;
using Sharp.Data.Databases.MySql;
using Sharp.Data.Dialects;
using Sharp.Data.Providers;

namespace Sharp.Data {
    public class SharpDbProviderFactory {
        public SharpDbConfig CreateSharpDbConfig(string databaseProviderName) {

            DbProviderFactory dbProviderFactory = GetDbProviderFactory(databaseProviderName);

            IDataProvider dataProvider;
            Dialect dialect;

            if (databaseProviderName == DataProviderNames.OracleOdp) {
                dataProvider = new OracleOdpProvider(dbProviderFactory);
                dialect = new OracleDialect();
            }
            else if (databaseProviderName == DataProviderNames.OracleClient) {
                dataProvider = new OracleClientProvider(dbProviderFactory);
                dialect = new OracleDialect();
            }
            else if (databaseProviderName == DataProviderNames.SqlServer) {
                dataProvider = new SqlProvider(dbProviderFactory);
                dialect = new SqlDialect();
            }
            else if (databaseProviderName == DataProviderNames.SqLite) {
                dataProvider = new SqLiteProvider(dbProviderFactory);
                dialect = new SqLiteDialect();
            }
            else if (databaseProviderName == DataProviderNames.MySql) {
                dataProvider = new MySqlProvider(dbProviderFactory);
                dialect = new MySqlDialect();
            }
            else {
                throw new ProviderNotFoundException("Could not find provider " + databaseProviderName);
            }

            SharpDbConfig config = new SharpDbConfig {
                DbProviderName = databaseProviderName,
                DataProvider = dataProvider,
                Dialect = dialect
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
