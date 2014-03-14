using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharp.Migrations;
using Sharp.Data.Schema;

namespace Northwind.Sharp.Migrations {
    public class _001_CreateTableFoo : ReversibleSchemaMigration {

		public override void Up() {
			Add.Table("Foo").WithColumns(
                Column.AutoIncrement("id").AsPrimaryKey(),
				Column.String("Name").Size(50).NotNull()
			);
		}
	}
}