using Sharp.Migrations;

namespace Sharp.Tests.ReversableMigrations {
    public class _001_Create_table_revfoo_with_comments : ReversableSchemaMigration {
        public override void Up() {
            Add.Table("revfoo")
               .WithColumns(
                   Column.AutoIncrement("id").Comment("This is the id"),
                   Column.String("name").NotNull().Comment("This is the name")
                );
            Add.Comment("This table is cool!").ToTable("revfoo");
        }
    }
}