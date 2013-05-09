using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _005_Add_column_to_table_revbar : ReversableSchemaMigration {
        public override void Up() {
            Add.Column(Column.String("newcol")).ToTable("revbar");
        }
    }
}