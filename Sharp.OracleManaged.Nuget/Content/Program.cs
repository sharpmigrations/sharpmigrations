using System.Configuration;
using log4net.Config;
using Sharp.Data.Databases;
using Sharp.Migrations.Runners;

namespace App4 {
    internal class Program {
        private static void Main(string[] args) {
            XmlConfigurator.Configure();
            var connString = ConfigurationManager.ConnectionStrings["Oracle.ManagedDataAccess.Client"].ConnectionString;
            var runner = new ConsoleRunner(connString, DataProviderNames.OracleManaged);
            runner.Start();
        }
    }
}