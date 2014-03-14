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
            Console.WriteLine("");
            Console.WriteLine("Sharp Migrator");
            Console.WriteLine("");
            PrintPlataform();

            _options = new Options();
            ParserResult<Options> result = Parser.Default.ParseArguments<Options>(_args);
            if (result.Errors.Any()) {
                Exit();
            }
            if (_args.Length == 0) {
                Console.WriteLine(_options.GetUsage());
                Exit();
            }
            _options = result.Value;
            SetSharpConfig();
            PrintDataSource(SharpFactory.Default.ConnectionString);
            Run();
        }


        private static void Exit(string message = null) {
            if (message != null) {
                Console.WriteLine(message);
            }
            Environment.Exit(0);
        }

        private static void SetSharpConfig() {
            //if (_options.ConnectionString == "") {
            //    if (_options.ConnectionStringName == "") {
            //        Exit("Please, set ");
            //    }
            //    _options.ConnectionString = ConfigurationManager.ConnectionStrings[connectionstring].ConnectionString;
            //}
            //if (_options.DatabaseProvider == "") {
            //    _options.DatabaseProvider = ConfigurationManager.ConnectionStrings[connectionstring].ProviderName;
            //}
            SharpFactory.Default.ConnectionString = _options.ConnectionString;
            SharpFactory.Default.DataProviderName = _options.DatabaseProvider;
        }

        private static void PrintPlataform() {
            int bits = IntPtr.Size == 4 ? 32 : 64;
            Console.WriteLine("Running in " + bits + " bits");
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
            var crunner = new ConsoleRunner(SharpFactory.Default.ConnectionString, SharpFactory.Default.DataProviderName);
            crunner.AssemblyWithMigrations = GetAssemblyWithMigrations();
            crunner.Start();
        }

        private static Assembly GetAssemblyWithMigrations() {
            if(String.IsNullOrEmpty(_options.AssemblyWithMigrations)) {
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
    }
}
