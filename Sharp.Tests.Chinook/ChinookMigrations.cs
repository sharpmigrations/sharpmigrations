using Sharp.Migrations.Runners;

namespace SharpMigrations.Tests.Chinook {
    public class ChinookMigrations : ChooseDbConsoleRunner {
        public ChinookMigrations()
            : base("", "", "Chinook") {
        }
    }
}