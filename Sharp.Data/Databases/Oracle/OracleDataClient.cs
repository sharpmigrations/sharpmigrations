namespace Sharp.Data.Databases.Oracle {
    public class OracleDataClient : DataClient {
        public OracleDataClient(IDatabase database) : base(database, new OracleDialect()) {
        }
    }
}