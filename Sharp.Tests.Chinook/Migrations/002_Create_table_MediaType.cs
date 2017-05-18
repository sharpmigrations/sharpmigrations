using Sharp.Data.Schema;
using Sharp.Migrations;

namespace SharpMigrations.Tests.Chinook {
    public class _002_Create_table_MediaType : SchemaMigration {
        public override void Up() {
            Add.Table("MediaType").WithColumns(
                Column.Int32("MediaTypeId").AsPrimaryKey(),
                Column.String("Name", 120)
            );
        }

        public override void Down() {
            Remove.Table("MediaType");
        }
    }
}