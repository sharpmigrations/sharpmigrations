using Sharp.Migrations;

namespace Sharp.Tests.Migrations {
    public class SeedForTest : SeedMigration {

        public static bool UpCalled { get; set; }
        public static string Param { get; set; }

        public static void Reset() {
            UpCalled = false;
            Param = null;
        }

        public override void Up(string param = null) {
            Param = param;
            UpCalled = true;
        }
    }
}