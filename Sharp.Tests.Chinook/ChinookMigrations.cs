using Sharp.Migrations.Runners;

namespace Sharp.Tests.Chinook {
    public class ChinookMigrations : ChooseDbConsoleRunner {
        public ChinookMigrations()
            : base("", "", "Chinook") {
        }
    }
}