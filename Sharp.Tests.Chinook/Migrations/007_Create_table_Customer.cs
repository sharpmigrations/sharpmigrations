using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    
    public class _007_Create_table_Customer : SchemaMigration {

        public override void Up() {
            Add.Table("Customer").WithColumns(
                Column.AutoIncrement("CustomerId").AsPrimaryKey(),
                Column.String("FirstName", 40).NotNull(),
                Column.String("LastName", 20).NotNull(),
                Column.String("Company", 80),
                Column.String("Address",70),
                Column.String("City",40),
                Column.String("State",40),
                Column.String("Country", 40),
                Column.String("PostalCode", 10),
                Column.String("Phone", 24),
                Column.String("Fax", 24),
                Column.String("Email", 60).NotNull(),
                Column.Int32("SupportRepId")
            );
        }

        public override void Down() {
            Remove.Table("Customer");
        }
    }
}
