namespace Sharp.Data.Databases.Oracle {
    public class OracleManagedDbFactory : DbFactory {
        public OracleManagedDbFactory(string databaseProviderName, string connectionString)
            : base(databaseProviderName, connectionString) {
        }

        public override IDataProvider CreateDataProvider() {
            return new OracleManagedProvider(GetDbProviderFactory(DatabaseProviderName));
        }
        
        public override Dialect CreateDialect() {
            return new OracleDialect();
        }

        public override IDataClient CreateDataClient() {
            return new OracleDataClient(CreateDatabase(), CreateDialect());
        }
    }
}