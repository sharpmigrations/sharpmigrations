
namespace SharpMigrations.Tests.ReversibleMigrations.Migrations {
    public class _002_Create_table_bar : ReversibleSchemaMigration {
        public override void Up() {
            Add.Table("rev")
               .WithColumns(
                   Column.AutoIncrement("id").AsPrimaryKey(),
                   Column.Int32("idrevfoo").NotNull(),
                   Column.String("name").NotNull()
                );
        }
    }
}