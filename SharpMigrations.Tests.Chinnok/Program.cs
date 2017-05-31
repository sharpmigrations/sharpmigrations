using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using SharpData;
using SharpMigrations.Runners;

namespace SharpMigrations.Tests.Chinnok {
    class Program {
        static void Main(string[] args) {
            var connectionString = ConfigurationManager.ConnectionStrings["System.Data.SqlClient"].ConnectionString;
            var factory = new SharpFactory(SqlClientFactory.Instance, connectionString);
            var consoleRunner = new ConsoleRunner(factory.CreateDataClient(), Assembly.GetExecutingAssembly());
            consoleRunner.Start();
        }
    }
}
