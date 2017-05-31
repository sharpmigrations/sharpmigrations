namespace SharpMigrations.Tests.Chinnok.Migrations {

    public class _008_Create_table_Involce : SchemaMigration {
        
        public override void Up() {
            Add.Table("Invoice").WithColumns(
                Column.AutoIncrement("InvoiceId").AsPrimaryKey(),
                Column.Int32("CustomerId").NotNull(),
                Column.Date("InvoiceDate").NotNull(),
                Column.String("BillingAddress", 70),
                Column.String("BillingCity", 40),
                Column.String("BillingState", 40),
                Column.String("BillingCountry", 40),
                Column.String("BillingPostalCode", 10),
                Column.Int32("Total").NotNull()
            );
        }

        public override void Down() {
            Remove.Table("Invoice");
        }
    }
}