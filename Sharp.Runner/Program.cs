using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sharp.Data;
using Sharp.Migrations.Runners;
using Sharp.Tests.Chinook;

namespace Sharp.Runner {
    class Program {
        static void Main(string[] args) {
            Assembly assembly = typeof (ChinookMigrations).Assembly;
            //var assembly = Assembly.LoadFile(@"..\..\Sharp.Tests.Chinook\bin\Release\Sharp.Tests.Chinook.exe");

            string providerName = "Oracle.ManagedDataAccess.Client";
            string connectionString = "Data Source=//localhost:1521/XE; User Id=sharp; Password=sharp;";
            var runner = new CmdRunner(SharpFactory.Default.CreateDataClient(connectionString, providerName), assembly);
            runner.Run(-1);

            foreach (var sql in runner.GetAllExecutedSqls()) {
                Console.WriteLine("--------------------------------------------------------------");
                Console.Write(sql);
            }
            File.WriteAllLines("sql.txt", runner.GetAllExecutedSqls());
        }
    }
}
