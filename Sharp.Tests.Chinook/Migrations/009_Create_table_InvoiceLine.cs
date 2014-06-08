using Sharp.Data.Schema;
using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _009_Create_table_InvoiceLine : SchemaMigration {

        public override void Up() {
            Add.Table("InvoiceLine").WithColumns(
                Column.AutoIncrement("InvoiceLineId").AsPrimaryKey(),
                Column.Int32("InvoiceId").NotNull(),
                Column.Int32("InvoiceDate").NotNull(),
                Column.Int32("TrackId").NotNull(),
                Column.Int32("UnitPrice").NotNull(),
                Column.Int32("Quantity").NotNull()
            );
        }

        public override void Down() {
            Remove.Table("InvoiceLine");
        }
    }
}
