using Sharp.Data.Log;
using Sharp.Integration.Log4net;
using log4net.Config;
using Sharp.Tests.Chinook;

namespace Sharp.Migrations {
	internal class Program {
		private static void Main(string[] args) {
			XmlConfigurator.Configure();
            LogManager.Configure(new Log4NetLoggerFactory());

			ChinookMigrations migrations = new ChinookMigrations();
			migrations.Start();
		}
	}
}