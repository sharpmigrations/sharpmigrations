using System.Configuration;
using System.Reflection;
using System.Text;
using SharpData;
using SharpData.Databases;
using SharpMigrations.Runners;

namespace SharpMigrator {
    public class ChooseDbConsoleRunner111 {
        private readonly Assembly _assembly;
        private readonly string _migrationGroup;
        private string _connectionString;
        public const string ASK_FOR_DATABASE = "Please, enter database to migrate:";

        private DbProviderType _dbProviderType;

        public ChooseDbConsoleRunner111(Assembly assembly, string migrationGroup = null) {
            _assembly = assembly;
            _migrationGroup = migrationGroup;
        }

        public void Start() {
            GetDatabaseType();
            GetConnectionString();
            var factory = DbFinder.GetSharpFactory(_dbProviderType, _connectionString);
            var runner = new ConsoleRunner(factory.CreateDataClient(), _assembly, _migrationGroup);
            runner.Start();
        }

        private void GetDatabaseType() {
            var menu = GetMenu();
            _dbProviderType = DbProviderTypeExtensions.GetAll()[ConsoleRunner.GetIntFromConsole(menu)];
        }

        private void GetConnectionString() {
            _connectionString = ConfigurationManager.ConnectionStrings[_dbProviderType.ToString()].ConnectionString;
        }

        private string GetMenu() {
            var menu = new StringBuilder();
            menu.AppendLine(ASK_FOR_DATABASE);
            var all = DbProviderTypeExtensions.GetAll();
            for (var i = 0; i < all.Count; i++) {
                menu.Append(i).Append(" - ").AppendLine(all[i].ToString());
            }
            return menu.ToString();
        }
    }
}