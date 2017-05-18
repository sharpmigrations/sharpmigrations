using Sharp.Migrations;

namespace SharpMigrations.Tests.Chinook {
    public class _005_Add_column_to_table_revbar : ReversibleSchemaMigration {
        public override void Up() {
            Add.Column(Column.String("newcol")).ToTable("revbar");
        }
    }
}