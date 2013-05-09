using Sharp.Migrations;

namespace Sharp.Tests.ReversableMigrations {
    public class _002_Create_table_bar : ReversableSchemaMigration {
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