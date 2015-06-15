namespace Sharp.Data.Databases.PostgreSql {
    public class PostgreDbFactory : DbFactory {
        public PostgreDbFactory(string databaseProviderName, string connectionString) : base(databaseProviderName, connectionString) { }
        public override IDataProvider CreateDataProvider() {
            return new PostgreSqlProvider(GetDbProviderFactory(DatabaseProviderName));
        }

        public override Dialect CreateDialect() {
            return new PostgreSqlDialect();
        }

        public override IDataClient CreateDataClient() {
            return new PostgreSqlDataClient(CreateDatabase(), CreateDialect());
        }
    }
}
