using Sharp.Data.Log;
using log4net.Config;
using Sharp.Tests.Chinook;

namespace Sharp.Migrations {
	internal class Program {
		private static void Main(string[] args) {
			XmlConfigurator.Configure();
			var migrations = new ChinookMigrations();
			migrations.Start();
		}
	}
}