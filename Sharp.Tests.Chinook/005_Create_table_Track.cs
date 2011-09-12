using Sharp.Data.Schema;
using Sharp.Migrations;

namespace Sharp.Tests.Chinook {

    public class _005_Create_table_Track : SchemaMigration {

        public override void Up() {
            Add.Table("Track").WithColumns(
                Column.AutoIncrement("TrackId").AsPrimaryKey(),
                Column.String("Name", 200).NotNull(),
                Column.Int32("AlbumId").NotNull(),
                Column.Int32("MediaTypeId").NotNull(),
                Column.Int32("GenreId"),
                Column.String("Composer", 220),
                Column.Int32("Milliseconds").NotNull(),
                Column.Int32("Bytes"),
                Column.Int32("UnitPrice").NotNull()
            );
        }

        public override void Down() {
            Remove.Table("Track");
        }
    }
}