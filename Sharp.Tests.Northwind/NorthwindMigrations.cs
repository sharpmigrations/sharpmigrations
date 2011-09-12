using Sharp.Migrations.Runners;

namespace Northwind.Sharp.Migrations {
    public class NorthwindMigrations : ChooseDbConsoleRunner {
        public NorthwindMigrations(string connectionString, string databaseProvider)
            : base(connectionString, databaseProvider) {
        }
    }
}