using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharp.Migrations;

namespace Sharp.Tests.Chinook {
    public class _015_Add_Comment_To_Table_Customer : SchemaMigration {
        public override void Up() {
            Add.Comment("This is the table Customer").ToTable("Customer");
            Add.Comment("This the first name").ToColumn("FIRSTNAME").OfTable("Customer");
        }

        public override void Down() {
            Remove.Comment.FromTable("Customer");
            Remove.Comment.FromColumn("FIRSTNAME").OfTable("Customer");
        }
    }
}
