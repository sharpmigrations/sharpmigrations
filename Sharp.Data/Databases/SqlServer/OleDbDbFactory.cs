namespace Sharp.Data.Databases.SqlServer {
    public class OleDbDbFactory : SqlServerDbFactory {
        public OleDbDbFactory(string databaseProviderName, string connectionString)
            : base(databaseProviderName, connectionString) {
        }
    }
}