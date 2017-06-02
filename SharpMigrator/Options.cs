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

        [Option('p', "provider", HelpText = "Database provider. Options are: OracleManaged, MySql, SqlServer, SqLite, OleDb and PostgreSql")]
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