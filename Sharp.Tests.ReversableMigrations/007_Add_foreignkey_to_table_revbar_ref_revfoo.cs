using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _007_Add_foreignkey_to_table_revbar_ref_revfoo : ReversibleSchemaMigration {
        public override void Up() {
            Add.ForeignKey("fk_revbar_revfoo").OnColumn("idrevfoo").OfTable("revbar").ReferencingColumn("id").OfTable("revfoo").OnDeleteCascade();
        }
    }
}