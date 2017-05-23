namespace SharpMigrations.Tests.Northwind {
    public class _002_CreateTableTerritories : SchemaMigration {
        public override void Up() {
            Add.Table("Territories").WithColumns(
                Column.String("TerritoryID").Size(20).NotNull(),
                Column.String("TerritoryDescription").Size(50).NotNull(),
                Column.Int32("RegionID").NotNull()
                );
            Add.PrimaryKey("PK_Territories").OnColumns("TerritoryID").OfTable("Territories");
        }

        public override void Down() {
            Remove.Table("Territories");
        }
    }
}