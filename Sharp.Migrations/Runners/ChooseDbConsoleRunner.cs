using System.Configuration;
using System.Reflection;
using System.Text;
using Sharp.Data;
using Sharp.Data.Databases;

namespace Sharp.Migrations.Runners {
    public class ChooseDbConsoleRunner : ConsoleRunner {
        public const string ASK_FOR_DATABASE = "Please, enter database to migrate:";

        public ChooseDbConsoleRunner(string connectionString, string databaseProvider)
            : base(connectionString, databaseProvider) {
        }

        protected override void GetInfoFromUser() {
            GetDatabaseType();
            GetTargetVersion();
            GetConnectionString();
        }

        private void GetDatabaseType() {
            string menu = GetMenu();
            DatabaseProvider = DataProviderNames.All[GetIntFromConsole(menu)];
        }

        private void GetConnectionString() {
            _connectionString = ConfigurationManager.ConnectionStrings[DatabaseProvider].ConnectionString;
        }

        private string GetMenu() {
            StringBuilder menu = new StringBuilder();
            menu.AppendLine(ASK_FOR_DATABASE);
            for (int i = 0; i < DataProviderNames.All.Count; i++) {
                menu.Append(i).Append(" - ").AppendLine(DataProviderNames.All[i]);
            }
            return menu.ToString();
        }

        protected override void TryRunMigrations() {
            IDataClientFactory dataClientFactory = new DataClientFactory();
            Runner runner = new Runner(dataClientFactory.GetDataClient(_connectionString, DatabaseProvider), Assembly.GetEntryAssembly());
            runner.Run(_targetVersion);
        }
    }
}