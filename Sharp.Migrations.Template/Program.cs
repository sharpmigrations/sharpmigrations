using System.Configuration;
using System.Reflection;
using Sharp.Migrations.Runners;

namespace Sharp.Migrations.Template {
	internal class Program {
		private static string _connectionString;

		private static void Main(string[] args) {
			GetConnectionString();
			StartRunner();
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