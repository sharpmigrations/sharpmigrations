using System.Configuration;
using System.Reflection;
using log4net.Config;
using Sharp.Data;
using Sharp.Migrations.Runners;

namespace Sharp.Migrations.Template {
	internal class Program {
		private static string _connectionString;

		private static void Main(string[] args) {
			ConfigureLog4net();
			GetConnectionString();
			StartRunner();
		}

		private static void ConfigureLog4net() {
			XmlConfigurator.Configure();
		}

		private static void StartRunner() {
			ConsoleRunner runner = new ConsoleRunner(_connectionString, DatabaseType.SqlServer);
			runner.AssemblyWithMigrations = Assembly.GetExecutingAssembly();
			runner.Start();
		}

		private static void GetConnectionString() {
			_connectionString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
		}
	}
}