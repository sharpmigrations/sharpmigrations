using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net.Config;
using SharpData;
using SharpMigrations.Runners;

namespace SharpMigrations.Tests.Northwind {
    class Program {
        static void Main(string[] args) {
            //XmlConfigurator.Configure();
            var connectionString = ConfigurationManager.ConnectionStrings["System.Data.SqlClient"].ConnectionString;
            var factory = new SharpFactory(SqlClientFactory.Instance, connectionString);
            var consoleRunner = new ConsoleRunner(factory.CreateDataClient(), Assembly.GetExecutingAssembly());
            consoleRunner.Start();
        }
    }
}
