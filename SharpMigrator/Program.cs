using System;
using System.IO;
using System.Reflection;
using CommandLine;
using SharpData;

namespace SharpMigrator {
    public class Program {
        public static Migrator Migrator;

        public static void Main(string[] args) {
            //string[] files = Directory.GetFiles(Path.GetFullPath("."), "*.dll");
            //foreach(string file in files) { File.Delete(file); }
            //args = "migrate".Split('|');
            //args = "migrate|-v|10".Split('|');
            //args = @"migrate|-a|..\..\..\SharpMigrations.Tests.Chinnok\bin\Debug\SharpMigrations.Tests.Chinnok.exe".Split('|');
            //args = @"migrate|-v|0|-a|..\..\..\SharpMigrations.Tests.Chinnok\bin\Debug\SharpMigrations.Tests.Chinnok.exe".Split('|');
            //args = @"script|-v|10|-a|..\..\..\SharpMigrations.Tests.Chinnok\bin\Debug\SharpMigrations.Tests.Chinnok.exe".Split('|');

            //args = @"migrate|-v|10|-a|..\..\..\SharpMigrations.Tests.Chinnok\bin\Debug\SharpMigrations.Tests.Chinnok.exe|-c|Data Source=//localhost:1521/XE;User Id=sharp;Password=sharp;".Split('|');
            //args = @"migrate|-v|-1|-a|..\..\..\SharpMigrations.Tests.Chinnok\bin\Debug\SharpMigrations.Tests.Chinnok.exe|-c|Server=(localdb)\mssqllocaldb;Database=sharp;Trusted_Connection=True;MultipleActiveResultSets=true|-p|sqlserver".Split('|');
            //args = @"migrate|-v|-1|-a|..\..\..\SharpMigrations.Tests.Chinnok\bin\Debug\SharpMigrations.Tests.Chinnok.exe|-c|User ID=sharp;Password=sharp;Host=localhost;Port=5432;Database=sharp;|-p|postgresql".Split('|');
            //args = @"seed|-s|SomeInserts|-a|..\..\..\SharpMigrations.Tests.Chinnok\bin\Debug\SharpMigrations.Tests.Chinnok.exe|-c|Server=(localdb)\mssqllocaldb;Database=sharp;Trusted_Connection=True;MultipleActiveResultSets=true|-p|sqlserver".Split('|');


            //SomeInserts
            //args = @"-a|..\..\..\SharpMigrations.Tests.Chinook\bin\Debug\SharpMigrations.Tests.Chinook.exe|-m|manual|-f|sql.txt|-v|-1|-c|Data Source=//localhost:1521/XE;User Id=sharp2;Password=sharp2;|-p|Oracle.ManagedDataAccess.Client".Split('|');
            //args = @"-a|..\..\..\SharpMigrations.Tests.Chinook\bin\Debug\SharpMigrations.Tests.Chinook.exe|-c|Data Source=//localhost:1521/XE;User Id=sharp;Password=sharp;|-p|Oracle.ManagedDataAccess.Client|-g|plugin|-m|script|-f|script.sql".Split('|');
            //args = @"-a|c:\dev\opensource\sharpmigrations\SharpMigrations.Tests.Northwind\bin\Debug\SharpMigrations.Tests.Northwind.exe".Split('|');
            //args = @"-a|C:\dev\test\TestApp\TestApp\bin\Debug\TestApp.exe|-p|Oracle.DataAccess.Client|-c|Data Source=//localhost:1521/XE; User Id=sharp; Password=sharp;|-m|auto|-v|0".Split('|');
            //args = @"-a|C:\dev\test\TestApp\TestApp\bin\Debug\TestApp.exe|-c|Data Source=//localhost:1521/XE; User Id=sharp; Password=sharp;|-m|auto".Split('|');
            //args = @"-a|C:\dev\test\TestApp\TestApp\bin\Debug\TestApp.exe|-c|Data Source=//localhost:1521/XE; User Id=sharp; Password=sharp;|-m|seed|-s|asdf".Split('|');
            //args = @"-a|C:\dev\test\TestApp\TestApp\bin\Debug\TestApp.exe|-p|Oracle.ManagedDataAccess.Client|-c|Data Source=//localhost:1521/XE; User Id=sharp; Password=sharp;".Split('|');

            //args = @"-a|C:\dev\way2\pim\Way2Pim.Migrations\bin\Debug\Way2Pim.Migrations.exe|-n|Oracle|-g|Way2Pim.Server|-m|script|-f|sql.txt".Split('|');
            //args = @"-a|C:\dev\way2\pim\Way2Pim.Migrations\bin\Debug\Way2Pim.Migrations.exe|-n|Oracle|-g|Way2Pim.Server|-v|201603300944".Split('|');
            //args = @"-a|C:\dev\opensource\Hangfire\src\HangFire.Oracle\bin\Debug\Hangfire.Oracle.dll|-p|Oracle.ManagedDataAccess.Client|-c|Data Source=//localhost:1521/XE;User Id=hangfire;Password=hangfire".Split('|');
            //args = @"seed|--assembly|Way2Pim.Migrations.exe|--seed|Crie_Pontos_Ccee|-i|20000|-c|""Data Source =//localhost:1521/XE;User Id=W2E_PIM;Password=W2E_PIM;""|-p|OracleManaged".Split('|');

            AppDomain.CurrentDomain.AssemblyResolve += ResolveNotFoundAssembly;
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Sharp Migrator v" + Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("--------------------------------");
            PrintPlataform();

            try {
                Migrator = new Migrator();
                Parser.Default.ParseArguments<MigrateOptions, ScriptOptions, SeedOptions>(args)
                    .WithParsed<MigrateOptions>(options => Migrator.Migrate(options))
                    .WithParsed<ScriptOptions>(options => Migrator.MigrateScript(options))
                    .WithParsed<SeedOptions>(options => Migrator.Seed(options))
                    .WithNotParsed(errors => {});
            }
            catch (Exception ex) {
                Console.WriteLine();
                Console.WriteLine("Error running migrator: ");
                Console.WriteLine(ExceptionHelper.GetAllErrors(ex));
            }
        }

        private static void PrintPlataform() {
            var bits = IntPtr.Size == 4 ? 32 : 64;
            Console.WriteLine("Running in    : " + bits + " bits");
            Console.WriteLine("--------------------------------");
        }

        public static Assembly ResolveNotFoundAssembly(object sender, ResolveEventArgs args) {
            var migrationsAssemblyPath = Migrator.Options.AssemblyWithMigrations;
            var applicationDirectory = Path.GetDirectoryName(migrationsAssemblyPath);

            var fields = args.Name.Split(',');
            var assemblyName = fields[0];
            var assemblyCulture = fields[2].Substring(fields[2].IndexOf('=') + 1);
            var assemblyFileName = assemblyName + ".dll";
            string assemblyPath;
            if (assemblyName.EndsWith(".resources")) {
                var resourceDirectory = Path.Combine(applicationDirectory, assemblyCulture);
                assemblyPath = Path.Combine(resourceDirectory, assemblyFileName);
            }
            else {
                assemblyPath = Path.Combine(applicationDirectory, assemblyFileName);
            }
            if (File.Exists(assemblyPath)) {
                var loadingAssembly = Assembly.LoadFrom(assemblyPath);
                return loadingAssembly;
            }
            return null;
        }
    }
}