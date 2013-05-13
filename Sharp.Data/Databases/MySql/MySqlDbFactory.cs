using Sharp.Data.Providers;

namespace Sharp.Data.Databases.MySql {
    public class MySqlDbFactory : DbFactory {
        public MySqlDbFactory(string databaseProviderName, string connectionString)
            : base(databaseProviderName, connectionString) {
        }

        public override IDataProvider CreateDataProvider() {
            return new MySqlProvider(GetDbProviderFactory(DatabaseProviderName));
        }

        public override Dialect CreateDialect() {
            return new MySqlDialect();
        }

        public override IDataClient CreateDataClient() {
            return new MySqlDataClient(CreateDatabase(), CreateDialect());
        }
    }
}