namespace SharpMigrations.Tests.Chinnok.Migrations {

    public class _004_Create_table_Album : SchemaMigration {

        public override void Up() {
            Add.Table("Album").WithColumns(
                    Column.AutoIncrement("AlbumId").AsPrimaryKey(),
                    Column.String("Title", 160).NotNull(),
                    Column.Int32("ArtistId").NotNull()
            );
        }

        public override void Down() {
            Remove.Table("Album");
        }
    }
}