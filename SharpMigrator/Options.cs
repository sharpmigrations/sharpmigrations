using CommandLine;
using CommandLine.Text;

namespace Sharp.Migrator {
    public class Options {
        [Option('a', "assembly", HelpText = "Assembly with migrations")]
        public string AssemblyWithMigrations { get; set; }

        [Option('n', "connectionstringname", HelpText = "This is the name of the connection string in app.config")]
        public string ConnectionStringName { get; set; }

        [Option('c', "connectionstring", HelpText = "This is the connection string of the target database")]
        public string ConnectionString { get; set; }

        [Option('p', "provider", HelpText = "This is the database provider", DefaultValue = "")]
        public string DatabaseProvider { get; set; }

        [Option('v', "version", HelpText = "Target version to migrate automatically to. Specifying -1 will migrate to the latest version")]
        public int? TargetVersion { get; set; }

        [Option('m', "mode", HelpText = "manual: prompt user for version. auto: run migrations without prompting the user (see parameter -v for version). script: generate scritps to the file specified on -f parameter.")]
        public string Mode { get; set; }

        [Option('f', "filename", HelpText = "name of the file when using --mode=script")]
        public string Filename { get; set; }

        [Option('g', "migrationgroup", HelpText = "name of target migration group")]
        public string MigrationGroup { get; set; }

        public string GetUsage() {
            var help = new HelpText("Usage:");
            help.AddPreOptionsLine("Ex: My.Migrations -m auto -v 10 -> Migrates to version 10 (no prompt)");
            help.AddPreOptionsLine("Ex: My.Migrations -m script -f script.sql -v 10 -> Generates scripts from current version to version 10 into script.sql file");
            help.AddPreOptionsLine("Ex: My.Migrations -m script -v 10 -g superplugin -> Generates scripts from current version to version 10 using migration group 'superplugin'");
            help.AddOptions(this);
            return help.ToString();
        }
    }
}