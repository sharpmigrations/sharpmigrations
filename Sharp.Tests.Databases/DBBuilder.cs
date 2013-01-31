using System.Configuration;
using Sharp.Data;
using Sharp.Data.Schema;

namespace Sharp.Tests.Databases {
    public static class DBBuilder {

        public static IDataClient GetDataClient(string databaseType) {
            string connectionString =
                ConfigurationManager.ConnectionStrings[databaseType].ConnectionString;
            return new SharpFactory().CreateDataClient(connectionString, databaseType);
        }
    }
}