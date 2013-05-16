using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _006_Add_primaryKey_to_table_revfoo : ReversibleSchemaMigration {
        public override void Up() {
            Add.PrimaryKey("pk_revfoo").OnColumns("id").OfTable("revfoo");
        }
    }
}