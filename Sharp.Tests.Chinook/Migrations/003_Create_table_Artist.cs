using Sharp.Data.Schema;
using Sharp.Migrations;

namespace SharpMigrations.Tests.Chinook {
    public class _003_Create_table_Artist : SchemaMigration {
        
        public override void Up() {
            
            Add.Table("Artist").WithColumns(
                Column.AutoIncrement("ArtistId").AsPrimaryKey(),
                Column.String("Name", 120)
            );
        }

        public override void Down() {
            Remove.Table("Artist");
        }
    }
}