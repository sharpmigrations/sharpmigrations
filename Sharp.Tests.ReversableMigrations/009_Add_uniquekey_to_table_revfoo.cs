using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _009_Add_uniquekey_to_table_revfoo : ReversibleSchemaMigration {
        public override void Up() {
            Add.UniqueKey("un_revfoo").OnColumns("name").OfTable("revfoo");
        }
    }
}