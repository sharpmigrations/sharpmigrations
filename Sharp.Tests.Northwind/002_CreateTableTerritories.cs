using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharp.Data.Schema;
using Sharp.Migrations;

namespace Northwind.Sharp.Migrations {
	public class _002_CreateTableTerritories : SchemaMigration {

		public override void Up() {
			Add.Table("Territories").WithColumns(
				Column.String("TerritoryID").WithSize(20).NotNull(),
				Column.String("TerritoryDescription").WithSize(50).NotNull(),
				Column.Int32("RegionID").NotNull()
			);
			Add.PrimaryKey("PK_Territories").OnColumns("TerritoryID").OfTable("Territories");
		}

		public override void Down() {
		}
	}
}