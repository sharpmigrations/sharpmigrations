using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _001_Create_table_genre : SchemaMigration {

        public override void Up() {

            Add.Table("Genre")
               .WithColumns(
                    Column.Int32("GenreId").AsPrimaryKey(),
                    Column.String("Name",120)
               );
        }

        public override void Down() {
            Remove.Table("Genre");
        }
    }
}