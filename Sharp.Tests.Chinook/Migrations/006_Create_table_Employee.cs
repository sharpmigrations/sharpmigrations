using Sharp.Data.Schema;
using Sharp.Migrations;

namespace SharpMigrations.Tests.Chinook {
    public class _006_Create_table_Employee : SchemaMigration {

        public override void Up() {
            Add.Table("Employee").WithColumns(
                Column.AutoIncrement("EmployeeId").AsPrimaryKey(),
                Column.String("LastName", 20).NotNull(),
                Column.String("FirstName", 20).NotNull(),
                Column.String("Title", 30),
                Column.Int32("ReportsTo"),
                Column.Date("BirthDate"),
                Column.Date("HireDate"),
                Column.String("Address", 70),
                Column.String("City", 40),
                Column.String("State", 40),
                Column.String("Country", 40),
                Column.String("PostalCode", 10),
                Column.String("Phone", 24),
                Column.String("Fax", 24),
                Column.String("Email", 60)
            );
        }

        public override void Down() {
            Remove.Table("Employee");
        }
    }
}