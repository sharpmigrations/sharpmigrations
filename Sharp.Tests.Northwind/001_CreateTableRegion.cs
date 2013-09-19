using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharp.Migrations;
using Sharp.Data.Schema;

namespace Northwind.Sharp.Migrations {
	public class _001_CreateTableRegion : SchemaMigration {

		public override void Up() {
			Add.Table("Region").WithColumns(
                Column.Int32("RegionID").NotNull(),
				Column.String("RegionDescription").Size(50).NotNull()
			);
			Add.PrimaryKey("PK_Region").OnColumns("RegionID").OfTable("Region");
		}

		public override void Down() {
			Remove.Table("Region");
		}
	}
}