namespace SharpMigrations.Tests.ReversibleMigrations.Migrations {
    public class _006_Add_primaryKey_to_table_revfoo : ReversibleSchemaMigration {
        public override void Up() {
            Add.PrimaryKey("pk_revfoo").OnColumns("id").OfTable("revfoo");
        }
    }
}