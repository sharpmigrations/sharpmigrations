namespace SharpMigrations.Tests.ReversibleMigrations.Migrations {
    public class _003_Rename_table_rev_to_revbar : ReversibleSchemaMigration {
        public override void Up() {
            Rename.Table("rev").To("revbar");
        }
    }
}