using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharp.Data;
using Sharp.Data.Schema;
using NUnit.Framework;
using Sharp.Data.Fluent;

namespace Sharp.Tests.Data {
    [TestFixture]
    public class FilterTests {

        [Test]
        public void Test() {
			IDataClient client = null;


			//client.Delete()
			//    .From("table")
			//    .Where
			//    .Eq("asdf", 1).And
			//    .Eq("asdf", 2).Or
			//    .Lt("asdf", 5)
			//    .DoIt();

			//client.Select()
			//    .AllColumns()
			//    .From("")
			//    .Where
			//    .Eq("asdf", 1).Or
			//    .Eq("asdf", 2)
			//    .DoIt();


			//client.Update.Table("table").SetColumns().ToValues();

			//client.WithTable("table")
			//      .WithAllRows
			//      .Select();

			//client.WithTable("table")
			//      .Where
			//      .Select();

			//client.WithTable("table")
			//      .AllRows
			//      .SetColumns()
			//      .ToValues();

			//client.WithTable("table")
			//      .WithAllRows
			//      .Delete();

			//client.WithTable("table")
			//      .Insert
			//      .IntoColumns()
			//      .TheValues();

			//Filter idEquals1AndNameEquals10 = Filter.Eq("Id", 1).And
			//                                        .Eq("Col",3).

			//client.Select.From("table").Where(idEquals1);

			//Filter filter1 = Filter.Eq("asdf", 1);
			//Filter filter2 = Filter.Eq("asdf", 2);

			//Filter filter = Filter.And(filter1, filter2);

			//Filter filter3 = Filter.Eq("asdf", 1);
			//Filter filter4 = Filter.Eq("asdf", 2);

			//Filter filter = Filter.Or(filter3, filter4);




			//client.Insert.Into("tab").Columns("asdf").Values("asdf");

			//client.Update.Table("asdf").SetColumns("").ToValues("asdf").Where(filter);


            //client.Delete
            //      .From("tablex")
            //      .Only.When.Column("id").Eq(1);

            //client.Delete
            //    .From("tbx")
            //    .Where(
            //        Filter.Eq("c1",1).OrFilter(
            //            Filter.Eq("c1",2).AndFilter(
            //                Filter.Eq("c2",3).AndFilter(
            //                    Filter.Eq("c1",1).Or.Eq("c2",2)
            //                )
            //            )
            //        )
            //    );
        }
    }
}
