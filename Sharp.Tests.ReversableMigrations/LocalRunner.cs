using System.Reflection;
using Sharp.Data;
using Sharp.Migrations;
using Sharp.Migrations.Runners;

namespace Sharp.Tests.ReversibleMigrations {
    public class LocalRunner : ChooseDbConsoleRunner {
        public LocalRunner()
            : base("", "") {
        }

        protected override void TryRunMigrations() {
            IDataClient dataClient = SharpFactory.Default.CreateDataClient(_connectionString, DatabaseProvider);
            var runner = new Runner(dataClient, Assembly.GetEntryAssembly());
            runner.Run(_targetVersion);
        }
    }
}