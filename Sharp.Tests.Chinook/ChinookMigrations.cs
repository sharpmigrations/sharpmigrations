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

            var database = new SqlToFileDatabase(null);

            IDataClient dataClient = SharpFactory.Default.CreateDataClient(_connectionString, DatabaseProvider);
            Runner runner = new Runner(dataClient, Assembly.GetEntryAssembly());
            runner.Run(_targetVersion);
        }
    }
}