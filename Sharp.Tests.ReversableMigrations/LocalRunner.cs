using Sharp.Migrations.Runners;

namespace SharpMigrations.Tests.ReversibleMigrations {
    public class LocalRunner : ChooseDbConsoleRunner {
        public LocalRunner()
            : base("", "", "reversible") {}
    }
}