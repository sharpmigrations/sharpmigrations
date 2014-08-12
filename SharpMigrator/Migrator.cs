using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandLine;
using Sharp.Data;
using Sharp.Migrations;
using Sharp.Migrations.Runners;
using Sharp.Migrations.Runners.ScriptCreator;

namespace Sharp.Migrator {
    public class Migrator {
        private readonly string[] _args;
        private static Options _options;

        public Migrator(string[] args) {
            _args = args;
        }

        public void Start() {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Sharp Migrator v" + Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("--------------------------------");
            PrintPlataform();

            _options = new Options();
            if (_args.Length == 0 || !Parser.Default.ParseArguments(_args, _options)) {
                Console.WriteLine(_options.GetUsage());
                Exit();
            }
            PrintMigrationGroup();
            SetSharpConfig();
            PrintDataSource(SharpFactory.Default.ConnectionString);
            Run();
        }

        private void PrintMigrationGroup() {
            string migrationGroup = String.IsNullOrEmpty(_options.MigrationGroup) ? "not set" : _options.MigrationGroup;            
            Console.WriteLine("MigrationGroup: " + migrationGroup);
            Console.WriteLine("--------------------------------"); 
        }


        private static void Exit(string message = null) {
            if (message != null) {
                Console.WriteLine(message);
            }
            Environment.Exit(0);
        }

        private static void SetSharpConfig() {
            if (SetConnectionString() || SetConnectionStringViaConnectionName() || SetConnectionStringViaAppConfig()) {
                SetSharpConfigFromOptions();
                return;
            }
            Exit("Please, set a connectionstring");                        
        }

        private static void SetSharpConfigFromOptions() {
            if (String.IsNullOrEmpty(_options.ConnectionString)) {
                Exit("Please, set a connectionstring");        
            }
            if (String.IsNullOrEmpty(_options.DatabaseProvider)) {
                Exit("Please, set a database provider");                        
            }
            SharpFactory.Default.ConnectionString = _options.ConnectionString;
            SharpFactory.Default.DataProviderName = _options.DatabaseProvider;
        }

        private static bool SetConnectionString() {
            return !String.IsNullOrEmpty(_options.ConnectionString) && !String.IsNullOrEmpty(_options.DatabaseProvider);
        }

        private static bool SetConnectionStringViaConnectionName() {
            if (String.IsNullOrEmpty(_options.ConnectionStringName)) {
                return false;
            }
            _options.ConnectionString = ConfigurationManager.ConnectionStrings[_options.ConnectionStringName].ConnectionString;
            _options.DatabaseProvider = ConfigurationManager.ConnectionStrings[_options.ConnectionStringName].ProviderName;
            return true;
        }
        private static bool SetConnectionStringViaAppConfig() {
            if (String.IsNullOrEmpty(_options.AssemblyWithMigrations)) {
                return false;
            }
            var appConfig = ConfigurationManager.OpenExeConfiguration(_options.AssemblyWithMigrations);
            var cs = GetConnectionStringSettings(appConfig.ConnectionStrings.ConnectionStrings);
            _options.ConnectionString = cs.ConnectionString;
            _options.DatabaseProvider = cs.ProviderName;
            return true;
        }

        private static void PrintPlataform() {
            int bits = IntPtr.Size == 4 ? 32 : 64;
            Console.WriteLine("Running in    : " + bits + " bits");
        }

        private static void PrintDataSource(string connectionString) {
            var start = connectionString.IndexOf("Data Source=") + "Data Source=".Length;
            var end = connectionString.IndexOf(@";", start);
            var dataSource = string.Format("Data Source: {0}", connectionString.Substring(start, end - start));
            Console.WriteLine(dataSource);
        }

        private static void Run() {
            int version = _options.TargetVersion ?? -1;
            string mode = (_options.Mode ?? "manual").ToLower();
            if (mode == "script") {
                RunScript(version);
                return;
            }
            if (mode == "auto") {
                var runner = new Runner(SharpFactory.Default.CreateDataClient(), GetAssemblyWithMigrations());
                runner.MigrationGroup = _options.MigrationGroup;
                runner.Run(version);
                return;
            }
            if (mode == "seed") {
                if (String.IsNullOrEmpty(_options.SeedName)) {
                    Exit("Please, set the seed name");
                    return;
                }
                var seedRunner = new SeedRunner(SharpFactory.Default.CreateDataClient(), GetAssemblyWithMigrations());
                seedRunner.Run(_options.SeedName, _options.SeedArgs, _options.MigrationGroup);
                return;
            }
            var crunner = new ConsoleRunner(SharpFactory.Default.ConnectionString, SharpFactory.Default.DataProviderName);
            crunner.AssemblyWithMigrations = GetAssemblyWithMigrations();
            crunner.MigrationGroup = _options.MigrationGroup;
            crunner.Start();
        }

        private static Assembly GetAssemblyWithMigrations() {
            if (String.IsNullOrEmpty(_options.AssemblyWithMigrations)) {
                return Assembly.GetEntryAssembly();
            }
            return Assembly.LoadFile(Path.GetFullPath(_options.AssemblyWithMigrations));
        }

        private static void RunScript(int version) {
            if (String.IsNullOrEmpty(_options.Filename)) {
                Exit();
            }
            var runner = new ScriptCreatorRunner(SharpFactory.Default.CreateDataClient(), GetAssemblyWithMigrations());
            runner.Run(version, _options.MigrationGroup);
            File.WriteAllText(_options.Filename, runner.GetCreatedScript(), Encoding.UTF8);
            Console.WriteLine(" * Check {0} for the script dump. No migrations were performed on the database.", _options.Filename);
        }

        private static ConnectionStringSettings GetConnectionStringSettings(ConnectionStringSettingsCollection section) {
            Console.WriteLine("You have to provide a connectionstring and a database provider. I couldn't detect them, so pick one from app.config:");
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
