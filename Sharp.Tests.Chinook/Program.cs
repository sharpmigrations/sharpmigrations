using log4net.Config;
using Sharp.Data.Config;
using Sharp.Tests.Chinook;

namespace Sharp.Migrations {
	internal class Program {
		private static void Main(string[] args) {
			XmlConfigurator.Configure();

			DefaultConfig.IgnoreDialectNotSupportedActions = true;

			ChinookMigrations migrations = new ChinookMigrations(DefaultConfig.ConnectionString, DefaultConfig.DatabaseProvider);
			migrations.Start();
		}
	}
}