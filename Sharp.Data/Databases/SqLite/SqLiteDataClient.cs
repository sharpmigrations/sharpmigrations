namespace Sharp.Data.Databases.SqLite {
    public class SqLiteDataClient : DataClient {
        public SqLiteDataClient(IDatabase database) : base(database, new SqLiteDialect()) {
        }
    }
}