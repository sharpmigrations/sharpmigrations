using System.Configuration;
using System.Reflection;
using Sharp.Data.Log;
using Sharp.Integration.Log4net;
using Sharp.Migrations.Runners;

namespace Sharp.Migrations.Template {
	internal class Program {
		private static string _connectionString;

		private static void Main(string[] args) {
			ConfigureLog();
			GetConnectionString();
			StartRunner();
		}

		private static void ConfigureLog() {
			LogManager.Configure(new Log4NetLoggerFactory());
		}

		private static void StartRunner() {
			var runner = new ConsoleRunner(_connectionString, "System.Data.SqlClient");
			runner.AssemblyWithMigrations = Assembly.GetExecutingAssembly();
			runner.Start();
		}

		private static void GetConnectionString() {
			_connectionString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
		}
	}
}