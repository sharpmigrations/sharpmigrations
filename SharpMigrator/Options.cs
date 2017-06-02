using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace SharpMigrator {
    public class Options {
        [Option('a', "assembly", HelpText = "Assembly with migrations", Required = true)]
        public string AssemblyWithMigrations { get; set; }

        [Option('n', "connectionstringname", HelpText = "This is the name of the connection string in app.config")]
        public string ConnectionStringName { get; set; }

        [Option('c', "connectionstring", HelpText = "This is the connection string of the target database")]
        public string ConnectionString { get; set; }

        [Option('p', "provider", HelpText = "This is the database provider")]
        public string DatabaseProvider { get; set; }

        [Usage(ApplicationAlias = "SharpMigrator")]
        public static IEnumerable<Example> Examples {
            get {
                yield return new Example("Migrates to the latest version (no prompt)", new MigrateOptions { TargetVersion = -1, AssemblyWithMigrations = "MyAssemblyWithMigrations.dll"});
                yield return new Example("Migrates using Oracle", new MigrateOptions { TargetVersion = -1, AssemblyWithMigrations = "MyAssemblyWithMigrations.dll", DatabaseProvider = "Oracle", ConnectionString = "my connectionstring"});
                yield return new Example("Generate scripts", new ScriptOptions { TargetVersion = 10, AssemblyWithMigrations = "MyAssemblyWithMigrations.dll", Filename = "mysql.txt"});
                yield return new Example("Apply seed to database", new SeedOptions { AssemblyWithMigrations = "MyAssemblyWithMigrations.dll", SeedName = "SomeInserts"});
            }
        }

        //public string GetUsage() {
        //    var help = new HelpText("Usage:");
        //    help.AddPreOptionsLine("Ex: SharpMigrator -a MyAssembly.dll -m auto -v 10 -> Migrates to version 10 (no prompt)");
        //    help.AddPreOptionsLine("Ex: SharpMigrator -a MyAssembly.dll -m script -f script.sql -v 10 -> Generates scripts from current version to version 10 into script.sql file");
        //    help.AddPreOptionsLine("Ex: SharpMigrator -a MyAssembly.dll -m script -v 10 -g superplugin -> Generates scripts from current version to version 10 using migration group 'superplugin'");
        //    help.AddPreOptionsLine("Ex: SharpMigrator -a MyAssembly.dll -m seed -s myseed -> Run seed named myseed");
        //    help.AddOptions(this);
        //    return help.ToString();
        //}
    }

    public class ExtraOptions : Options {
        [Option('v', "targetversion", HelpText = "Target version to migrate automatically to. Specifying -1 will migrate to the latest version", Required = true)]
        public long TargetVersion { get; set; }
        [Option('g', "migrationgroup", HelpText = "name of target migration group")]
        public string MigrationGroup { get; set; }
    }

    [Verb("migrate", HelpText = "Migrate database to target version")]
    public class MigrateOptions : ExtraOptions {
        
    }

    [Verb("script", HelpText = "Create migration scripts")]
    public class ScriptOptions : ExtraOptions {
        [Option('f', "filename", HelpText = "name of the file for script generation", Default = "migrations.sql")]
        public string Filename { get; set; }
    }

    [Verb("seed", HelpText = "Seed database with target seed")]
    public class SeedOptions : Options {
        [Option('s', "seed", HelpText = "seed name", Required = true)]
        public string SeedName { get; set; }
        [Option('i', "args", HelpText = "optinal parameter for seed")]
        public string SeedArgs { get; set; }
    }
    
}