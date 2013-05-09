using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _020_Add_primaryKey_to_table_superfoo_reversable : ReversableSchemaMigration {
        public override void Up() {
            Add.PrimaryKey("pk_superfoo").OnColumns("ID").OfTable("superfoo2");
        }
    }
}