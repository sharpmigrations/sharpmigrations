using System;
using System.Configuration;
using System.Reflection;

namespace Sharp.Migrations.Runners {
    public class FromAppConfigRunner : IRunner {
        private readonly string _assemblyPath;
        private ConsoleRunner _consoleRunner;

        public FromAppConfigRunner(string assemblyPath) {
            _assemblyPath = assemblyPath;
        }

        public void Run(long targetVersion, string migrationGroup = null) {
            var appConfig = ConfigurationManager.OpenExeConfiguration(_assemblyPath);
            var cs = GetConnectionStringSettings(appConfig.ConnectionStrings.ConnectionStrings);
            _consoleRunner = new ConsoleRunner(cs.ConnectionString, cs.ProviderName);
            _consoleRunner.AssemblyWithMigrations = Assembly.LoadFile(_assemblyPath);
            if (migrationGroup != null) {
                _consoleRunner.Start();
            }
            _consoleRunner.Start();
        }

        private static ConnectionStringSettings GetConnectionStringSettings(ConnectionStringSettingsCollection section) {
            for (var i = 1; i < section.Count; i++) {
                Console.WriteLine("{0} - {1}", i, ConfigurationManager.ConnectionStrings[i].Name);
                Console.WriteLine(section[i].ConnectionString);
                Console.WriteLine();
            }
            int index;
            while (!Int32.TryParse(Console.ReadLine(), out index)) {
                Console.WriteLine(ConsoleRunner.INVALID_NUMBER);
            }
            return ConfigurationManager.ConnectionStrings[index];
        }
    }
}