namespace Sharp.Data.Databases.Oracle {
    public class OracleOdpDbFactory : OracleManagedDbFactory {
        public OracleOdpDbFactory(string databaseProviderName, string connectionString)
            : base(databaseProviderName, connectionString) {
        }

        public override IDataProvider CreateDataProvider() {
            return new OracleOdpProvider(GetDbProviderFactory(DatabaseProviderName));
        }
    }
}