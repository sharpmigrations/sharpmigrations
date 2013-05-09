using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _018_Rename_column_of_table_superfoo2 : SchemaMigration {
        public override void Up() {
            Rename.Column("Name").OfTable("superfoo2").To("Name2");
        }

        public override void Down() {
            Rename.Column("Name2").OfTable("superfoo2").To("Name");
        }
    }
}