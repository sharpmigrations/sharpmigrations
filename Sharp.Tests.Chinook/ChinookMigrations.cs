using System.Reflection;
using Sharp.Data;
using Sharp.Migrations;
using Sharp.Migrations.Runners;

namespace Sharp.Tests.Chinook {
    public class ChinookMigrations : ChooseDbConsoleRunner {
        public ChinookMigrations()
            : base("", "") {
        }

        protected override void TryRunMigrations() {
            IDataClient dataClient = SharpFactory.Default.CreateDataClient(_connectionString, DatabaseProvider);
            var runner = new Runner(dataClient, Assembly.GetEntryAssembly());
            runner.MigrationGroup = "asdf";
            runner.Run(_targetVersion);
        }
    }
}