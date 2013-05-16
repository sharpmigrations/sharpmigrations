using Sharp.Data.Log;
using Sharp.Integration.Log4net;
using log4net.Config;

namespace Sharp.Tests.ReversibleMigrations {
    internal class Program {
        private static void Main(string[] args) {
            XmlConfigurator.Configure();
            LogManager.Configure(new Log4NetLoggerFactory());
            var migrations = new LocalRunner();
            migrations.Start();
        }
    }
}