using Sharp.Migrations.Runners;

namespace Sharp.Tests.Chinook {
    public class ChinookMigrations : ChooseDbConsoleRunner {
        public ChinookMigrations(string connectionString, string databaseProvider)
            : base(connectionString, databaseProvider) {
        }
    }
}