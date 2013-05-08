using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _018_Rename_column_of_table_foo : SchemaMigration {
        public override void Up() {
            Rename.Column("Name").OfTable("foo2").To("Name2");
        }

        public override void Down() {
            Rename.Column("Name2").OfTable("foo2").To("Name");
        }
    }
}