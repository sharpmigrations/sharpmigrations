using log4net.Config;
using Sharp.Tests.Chinook;

namespace Sharp.Migrations {
	internal class Program {
		private static void Main(string[] args) {
			XmlConfigurator.Configure();

			ChinookMigrations migrations = new ChinookMigrations();
			migrations.Start();
		}
	}
}