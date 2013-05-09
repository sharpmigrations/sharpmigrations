using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _016_Create_table_superfoo_with_comments : SchemaMigration {
        public override void Up() {
            Add.Table("superfoo")
               .WithColumns(
                   Column.AutoIncrement("ID").Comment("This is the ID"),
                   Column.String("Name").NotNull().Comment("This is the name")
                );
            Add.Comment("This table is cool!").ToTable("superfoo");
        }

        public override void Down() {
            Remove.Table("superfoo");
        }
    }
}