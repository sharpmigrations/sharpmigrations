using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _004_Create_table_Album : SchemaMigration {

        public override void Up() {

            Add.Table("Album").WithColumns(
                    Column.AutoIncrement("AlbumId"),
                    Column.String("Title", 160).NotNull(),
                    Column.Int32("ArtistId").NotNull()
            );

            Add.PrimaryKey("PK_ProductItem")
               .OnColumns("AlbumId")
               .OfTable("Album");
        }

        public override void Down() {
            Remove.Table("Album");
        }
    }
}