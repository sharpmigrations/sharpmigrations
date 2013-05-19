using Sharp.Data.Log;
using log4net.Config;

namespace Sharp.Tests.ReversibleMigrations {
    internal class Program {
        private static void Main(string[] args) {
            XmlConfigurator.Configure();
            var migrations = new LocalRunner();
            migrations.Start();
        }
    }
}