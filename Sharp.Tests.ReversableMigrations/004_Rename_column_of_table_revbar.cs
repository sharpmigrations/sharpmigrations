using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _004_Rename_column_of_table_revbar : ReversableSchemaMigration {
        public override void Up() {
            Rename.Column("name").OfTable("revbar").To("fullname");
        }
    }
}