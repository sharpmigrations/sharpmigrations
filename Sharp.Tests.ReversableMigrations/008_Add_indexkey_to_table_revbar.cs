using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _008_Add_indexkey_to_table_revbar : ReversibleSchemaMigration {
        public override void Up() {
            Add.IndexKey("in_revfoo").OnColumns("fullname").OfTable("revbar");
        }
    }
}