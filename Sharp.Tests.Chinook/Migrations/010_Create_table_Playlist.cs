using Sharp.Data.Schema;
using Sharp.Migrations;

namespace SharpMigrations.Tests.Chinook {

    public class _010_Create_table_Playlist : SchemaMigration {

        public override void Up() {
            Add.Table("Playlist").WithColumns(
                Column.AutoIncrement("PlaylistId").AsPrimaryKey(),
                Column.String("Name",120)
            );
        }

        public override void Down() {
            Remove.Table("Playlist");
        }
    }
}
