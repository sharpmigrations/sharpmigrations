using Sharp.Data.Providers;

namespace Sharp.Data.Databases.SqlServer {
    public class SqlServerDbFactory : DbFactory {
        public SqlServerDbFactory(string databaseProviderName, string connectionString)
            : base(databaseProviderName, connectionString) {
        }

        public override IDataProvider CreateDataProvider() {
            return new SqlProvider(GetDbProviderFactory(DatabaseProviderName));
        }

        public override Dialect CreateDialect() {
            return new SqlDialect();
        }

        public override IDataClient CreateDataClient() {
            return new SqlServerDataClient(CreateDatabase(), CreateDialect());
        }
    }
}