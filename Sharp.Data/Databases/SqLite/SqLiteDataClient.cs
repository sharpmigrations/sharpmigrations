namespace Sharp.Data.Databases.SqLite {
    public class SqLiteDataClient : DataClient {
        public SqLiteDataClient(IDatabase database, Dialect dialect)
            : base(database, dialect) {
        }
    }
}