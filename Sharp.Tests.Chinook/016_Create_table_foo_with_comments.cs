using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _016_Create_table_foo_with_comments : SchemaMigration {
        public override void Up() {
            Add.Table("foo")
               .WithColumns(
                   Column.AutoIncrement("ID").AsPrimaryKey().Comment("This is the ID"),
                   Column.String("Name").NotNull().Comment("This is the name")
                );
            Add.Comment("This table is cool!").ToTable("foo");
        }

        public override void Down() {
            Remove.Table("foo");
        }
    }
}