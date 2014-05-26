using Sharp.Migrations.Runners;

namespace Sharp.Tests.ReversibleMigrations {
    public class LocalRunner : ChooseDbConsoleRunner {
        public LocalRunner()
            : base("", "", "reversible") {}
    }
}