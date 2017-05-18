using Sharp.Migrations;

namespace SharpMigrations.Tests.Chinook {
    public class _004_Rename_column_of_table_revbar : ReversibleSchemaMigration {
        public override void Up() {
            Rename.Column("name").OfTable("revbar").To("fullname");
        }
    }
}