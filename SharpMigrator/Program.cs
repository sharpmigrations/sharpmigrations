using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net.Config;

namespace Sharp.Migrator {
    public class Program {
        public static void Main(string[] args) {
            //string[] files = Directory.GetFiles(Path.GetFullPath("."), "*.dll");
            //foreach(string file in files) { File.Delete(file); }
            //args = @"-a|..\..\..\Sharp.Tests.Chinook\bin\Debug\Sharp.Tests.Chinook.exe|-m|manual|-f|sql.txt|-v|-1|-c|Data Source=//localhost:1521/XE;User Id=sharp2;Password=sharp2;|-p|Oracle.ManagedDataAccess.Client".Split('|');
            //args = @"-a|..\..\..\Sharp.Tests.Chinook\bin\Debug\Sharp.Tests.Chinook.exe|-c|Data Source=//localhost:1521/XE;User Id=sharp;Password=sharp;|-p|Oracle.ManagedDataAccess.Client|-g|plugin|-m|script|-f|script.sql".Split('|');
            //args = @"-a|c:\dev\opensource\sharpmigrations\Sharp.Tests.Northwind\bin\Debug\Sharp.Tests.Northwind.exe".Split('|');
            //args = @"-a|C:\dev\test\TestApp\TestApp\bin\Debug\TestApp.exe|-p|Oracle.ManagedDataAccess.Client|-c|Data Source=//localhost:1521/XE; User Id=sharp; Password=sharp;|-m|auto|-v|0".Split('|');
            XmlConfigurator.Configure();
            var m = new Migrator(args);
            m.Start();
        }

        //private static void SetResolveAssembliesStrategy() {
        //    AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {
        //        String resourceName = "Sharp.Migrator.EmbeddedLibs." +
        //                              new AssemblyName(args.Name).Name + ".dll";
        //        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)) {
        //            var assemblyData = new Byte[stream.Length];
        //            stream.Read(assemblyData, 0, assemblyData.Length);
        //            return Assembly.Load(assemblyData);
        //        }
        //    };
        //}
    }
}