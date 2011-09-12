namespace Sharp.Data {
    public interface IDataClientFactory {
        IDataClient GetDataClient(string connectionString, string databaseProviderName);
        IDataClient GetDefaultDataClient();
    }
}
