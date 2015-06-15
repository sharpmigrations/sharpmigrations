namespace Sharp.Data.Databases.PostgreSql {
    public class PostgreSqlDataClient : DataClient {
        public PostgreSqlDataClient(IDatabase database, Dialect dialect) : base(database, dialect) { }
    }
}
