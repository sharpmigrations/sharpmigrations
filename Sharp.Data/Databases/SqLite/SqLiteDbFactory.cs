using Sharp.Data.Providers;

namespace Sharp.Data.Databases.SqLite {
    public class SqLiteDbFactory : DbFactory {
        public SqLiteDbFactory(string databaseProviderName, string connectionString)
            : base(databaseProviderName, connectionString) {
        }

        public override IDataProvider CreateDataProvider() {
            return new SqLiteProvider(GetDbProviderFactory(DatabaseProviderName));
        }

        public override Dialect CreateDialect() {
            return new SqLiteDialect();
        }

        public override IDataClient CreateDataClient() {
            return new SqLiteDataClient(CreateDatabase(), CreateDialect());
        }
    }
}