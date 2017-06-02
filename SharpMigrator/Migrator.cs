using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SharpData;
using SharpData.Databases;
using SharpMigrations;
using SharpMigrations.Runners;
using SharpMigrations.Runners.ScriptCreator;

namespace SharpMigrator {
    
    public class Migrator {

        public Options Options { get; private set; }

        public void Migrate(MigrateOptions options) {
            Prepare(options, "Migrate");
            PrintMigrationGroup(options);

            var crunner = new Runner(SharpFactory.Default.CreateDataClient(), GetAssemblyWithMigrations(options), options.MigrationGroup);
            crunner.Run(options.TargetVersion);
        }
        public void MigrateScript(ScriptOptions options) {
            Prepare(options, "Script");
            PrintMigrationGroup(options);

            var runner = new ScriptCreatorRunner(SharpFactory.Default.CreateDataClient(), GetAssemblyWithMigrations(options), options.MigrationGroup);
            runner.Run(options.TargetVersion);
            File.WriteAllText(options.Filename, runner.GetCreatedScript(), Encoding.UTF8);
            Console.WriteLine(
                $" * Check {options.Filename} for the script dump. No migrations were performed this time.");
        }

        public void Seed(SeedOptions options) {
            Prepare(options, "Seed");
            var seedRunner = new SeedRunner(SharpFactory.Default.CreateDataClient(), GetAssemblyWithMigrations(options));
            seedRunner.Run(options.SeedName, options.SeedArgs);
        }

        private void Prepare(Options options, string script) {
            Options = options;
            Print(script + " mode");
            SetSharpConfig(options);
            PrintDataSource(SharpFactory.Default.ConnectionString);
        }

        private static void Print(string info) {
            Console.WriteLine(info);
            Console.WriteLine("--------------------------------");
        }

        private void SetSharpConfig(Options options) {
            if (TrySetConnectionStringFromArgs(options) || TrySetConnectionStringFromConfigFile(options)) {
                SetSharpConfigFromOptions(options);
                return;
            }
            throw new ArgumentException("Please, set a connectionstring");
        }

        private bool TrySetConnectionStringFromArgs(Options options) {
            return !String.IsNullOrEmpty(options.ConnectionString) && !String.IsNullOrEmpty(options.DatabaseProvider);
        }

        private bool TrySetConnectionStringFromConfigFile(Options options) {
            if (String.IsNullOrEmpty(options.AssemblyWithMigrations)) {
                return false;
            }
            var appConfig = ConfigurationManager.OpenExeConfiguration(options.AssemblyWithMigrations);
            return SetConnectionStringViaConnectionName(options, appConfig) || SetConnectionStringViaAppConfig(options, appConfig);
        }

        private bool SetConnectionStringViaConnectionName(Options options, Configuration appConfig) {
            if (String.IsNullOrEmpty(options.ConnectionStringName)) {
                return false;
            }
            options.ConnectionString =
                appConfig.ConnectionStrings.ConnectionStrings[options.ConnectionStringName].ConnectionString;
            options.DatabaseProvider =
                appConfig.ConnectionStrings.ConnectionStrings[options.ConnectionStringName].ProviderName;
            return true;
        }

        private bool SetConnectionStringViaAppConfig(Options options, Configuration appConfig) {
            var cs = GetConnectionStringSettings(appConfig.ConnectionStrings.ConnectionStrings);
            options.ConnectionString = cs.ConnectionString;
            options.DatabaseProvider = cs.ProviderName;
            return true;
        }

        private void SetSharpConfigFromOptions(Options options) {
            if (String.IsNullOrEmpty(options.ConnectionString)) {
                throw new ConfigurationErrorsException("Please, set a connectionstring");
            }
            if (String.IsNullOrEmpty(options.DatabaseProvider)) {
                throw new ConfigurationErrorsException("Please, set a database provider");
            }
            var databaseProvider = GetByName(options.DatabaseProvider);
            SharpFactory.Default = DbFinder.GetSharpFactory(databaseProvider, options.ConnectionString);
            Console.WriteLine("Database Provider: " + databaseProvider);
            Console.WriteLine("--------------------------------");
        }

        private DbProviderType GetByName(string name) {
            foreach (var dbProviderType in DbProviderTypeExtensions.GetAll()) {
                if (String.Equals(dbProviderType.GetProviderName(), name, StringComparison.CurrentCultureIgnoreCase) ||
                    String.Equals(dbProviderType.ToString(), name, StringComparison.CurrentCultureIgnoreCase)
                    ) {
                    return dbProviderType;
                }
            }
            var options = String.Join(Environment.NewLine, DbProviderTypeExtensions.GetAll().Select(p => p.ToString()));
            throw new ArgumentException($"Could not find database provider named {name}. Available options are: {Environment.NewLine}{options}" );
        }

        private void PrintMigrationGroup(ExtraOptions options) {
            var migrationGroup = String.IsNullOrEmpty(options.MigrationGroup) ? VersionRepository.DEFAULT_MIGRATION_GROUP : options.MigrationGroup;
            Console.WriteLine("MigrationGroup: " + migrationGroup);
            Console.WriteLine("--------------------------------");
        }

        private static void PrintDataSource(string connectionString) {
            Console.WriteLine("ConnectionString: " + connectionString);
            Console.WriteLine("--------------------------------");
        }

        private Assembly GetAssemblyWithMigrations(Options options) {
            if (String.IsNullOrEmpty(options.AssemblyWithMigrations)) {
                return Assembly.GetEntryAssembly();
            }
            return Assembly.LoadFile(Path.GetFullPath(options.AssemblyWithMigrations));
        }

        private static ConnectionStringSettings GetConnectionStringSettings(ConnectionStringSettingsCollection section) {
            Console.WriteLine(
                "You have to provide a connectionstring and a database provider. I couldn't detect them, so pick one from app.config:");
            Console.WriteLine("");
            for (var i = 0; i < section.Count; i++) {
                Console.WriteLine("{0} - {1}", i, section[i].Name);
                Console.WriteLine(section[i].ConnectionString);
                Console.WriteLine();
            }
            Console.Write("> ");
            int index;
            while (!Int32.TryParse(Console.ReadLine(), out index)) {
                Console.WriteLine(ConsoleRunner.INVALID_NUMBER);
            }
            Console.WriteLine("");
            Console.WriteLine("--------------------------------");
            return section[index];
        }
    }
}