using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _003_Rename_table_rev_to_revbar : ReversableSchemaMigration {
        public override void Up() {
            Rename.Table("rev").To("revbar");
        }
    }
}