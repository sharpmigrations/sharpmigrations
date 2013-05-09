using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _017_Rename_table_superfoo : SchemaMigration {
        public override void Up() {
            Rename.Table("superfoo").To("superfoo2");
        }

        public override void Down() {
            Rename.Table("superfoo2").To("superfoo");
        }
    }
}