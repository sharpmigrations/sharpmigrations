using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _017_Rename_table_foo : SchemaMigration {
        public override void Up() {
            Rename.Table("foo").To("foo2");
        }

        public override void Down() {
            Rename.Table("foo2").To("foo");
        }
    }
}