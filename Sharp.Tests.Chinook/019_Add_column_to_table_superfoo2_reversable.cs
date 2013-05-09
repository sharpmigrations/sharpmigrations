using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _019_Add_column_to_table_foo_reversable : ReversableSchemaMigration {
        public override void Up() {
            Add.Column(Column.String("SuperCol")).ToTable("superfoo2");
        }
    }
}