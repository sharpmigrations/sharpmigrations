namespace Sharp.Data {
    public interface ISharpFactory {
        string ConnectionString { get; set; }
        string DataProviderName { get; set; }

        IDataProvider CreateDataProvider(string databaseProviderName);
        IDataProvider CreateDataProvider();

        IDatabase CreateDatabase(string connectionString, string databaseProviderName);
        IDatabase CreateDatabase();

        IDataClient CreateDataClient(string connectionString, string databaseProviderName);
        IDataClient CreateDataClient();
    }
}