using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;
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
            if (!Parser.Default.ParseArguments(_args, _options)) {
                Exit();
            }
            SetSharpConfig();
            PrintDataSource(SharpFactory.Default.ConnectionString);
            Run();
        }


        private static void Exit() {
            Environment.Exit(0);
        }

        private static void SetSharpConfig() {
            string connectionstringname = _options.ConnectionStringName;

            if (String.IsNullOrEmpty(_options.ConnectionString)) {
                _options.ConnectionString = ConfigurationManager.ConnectionStrings[connectionstringname].ConnectionString;
            }
            if (String.IsNullOrEmpty(_options.DatabaseProvider)) {
                _options.DatabaseProvider = ConfigurationManager.ConnectionStrings[connectionstringname].ProviderName;
            }
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
                new Runner(SharpFactory.Default.CreateDataClient(), GetAssemblyWithMigrations()).Run(version);
                return;
            }
            var runner = new ConsoleRunner(SharpFactory.Default.ConnectionString, SharpFactory.Default.DataProviderName);
            runner.AssemblyWithMigrations = GetAssemblyWithMigrations();
            runner.Start();
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
            runner.Run(version);
            File.WriteAllText(_options.Filename, runner.GetCreatedScript(), Encoding.UTF8);
            Console.WriteLine(" * Check {0} for the script dump. No migrations were performed on the database.", _options.Filename);
        }
    }

    public class Options {
        [Option('a', "assembly", HelpText = "Assembly with migrations")]
        public string AssemblyWithMigrations { get; set; }

        [Option('n', "connectionstringname", HelpText = "This is the name of the connection string in app.config")]
        public string ConnectionStringName { get; set; }

        [Option('c', "connectionstring", HelpText = "This is the connection string of the target database (default")]
        public string ConnectionString { get; set; }

        [Option('p', "provider", HelpText = "This is the database provider")]
        public string DatabaseProvider { get; set; }

        [Option('v', "version", HelpText = "Target version to migrate automatically to. Specifying -1 will migrate to the latest version")]
        public int? TargetVersion { get; set; }

        [Option('m', "mode", HelpText = "manual: prompt user for version. auto: run migrations without prompting the user (see parameter -v for version). script: generate scritps to the file specified on -f parameter.")]
        public string Mode { get; set; }

        [Option('f', "filename", HelpText = "name of the file when using --mode=script")]
        public string Filename { get; set; }

        [HelpOption]
        public string GetUsage() {
            var help = new HelpText("Usage:");
            help.AddPreOptionsLine("Ex: Ccee.Migrations -m auto -v 10 -> Migrates to version 10 (no prompt)");
            help.AddPreOptionsLine("Ex: Ccee.Migrations -m script -f script.sql -v 10 -> Generates scripts from current version to version 10 into script.sql file");
            help.AddOptions(this);

            return help.ToString();
        }
    }
}
