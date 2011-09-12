using System.Configuration;
using Sharp.Data;

namespace Sharp.Tests.Databases {
    public static class DBBuilder {

        public static IDataClient GetDataClient(string databaseType) {
            string connectionString =
                ConfigurationManager.ConnectionStrings[databaseType].ConnectionString;
            return new DataClientFactory().GetDataClient(connectionString, databaseType);
        }
    }
}