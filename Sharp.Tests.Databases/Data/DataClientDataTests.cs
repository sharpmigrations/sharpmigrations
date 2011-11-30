using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Filters;
using Sharp.Data.Schema;

namespace Sharp.Tests.Databases.Data {
	
	[Explicit]
	public abstract class DataClientDataTests : DataClientTests {

		[Test]
		public virtual void Can_insert_returning_id() {
			_dataClient.AddTable("footable",
								 Column.AutoIncrement("id"),
								 Column.String("name")
				);

			Assert.AreEqual(1, _dataClient.Insert.Into("footable").Columns("name").ValuesAnd("asdf").Return<int>("id"));
			Assert.AreEqual(2, _dataClient.Insert.Into("footable").Columns("name").ValuesAnd("asdf").Return<int>("id"));
			Assert.AreEqual(3, _dataClient.Insert.Into("footable").Columns("name").ValuesAnd("asdf").Return<int>("id"));
		}

		[Test]
		public virtual void Can_insert_dates_and_booleans() {
			_dataClient.AddTable("footable",
								 Column.AutoIncrement("id"),
								 Column.Date("colDate"),
								 Column.Boolean("colBool"));


			DateTime now = DateTime.Now;

			_dataClient.Insert.Into("footable").Columns("colDate", "colBool").Values(now, true);

			ResultSet res = _dataClient.Select.Columns("colDate", "colBool").From("footable").AllRows();

			Assert.AreEqual(now.ToString(), res[0][0].ToString());

			Assert.AreEqual(true, res[0][1]);
		}

        [Test]
        public virtual void Can_insert_ints_and_strings() {
            _dataClient.AddTable("footable",
                                 Column.Int32("colInt"),
                                 Column.String("colString"));

            _dataClient.Insert.Into("footable").Columns("colInt", "colString").Values(1, "asdf");

            ResultSet res = _dataClient.Select.Columns("colInt", "colString").From("footable").AllRows();

            Assert.AreEqual(1, res[0][0]);
            Assert.AreEqual("asdf", res[0][1]);
        }

		[Test]
		public virtual void Can_insert_with_only_null() {
			_dataClient.AddTable("footable",
								 Column.AutoIncrement("id"),
								 Column.String("name")
			);

			_dataClient.Insert.Into("footable").Columns("name").Values(null);

			ResultSet res = _dataClient.Select.Columns("name").From("footable").AllRows();

			Assert.IsNull(res[0][0]);
		}

		[Test]
		public virtual void Can_insert_with_values_plus_null() {
			_dataClient.AddTable("footable",
								 Column.AutoIncrement("id"),
								 Column.String("name"),
								 Column.String("surname")
			);

			_dataClient.Insert.Into("footable").Columns("name", "surname").Values("foo", null);

			ResultSet res = _dataClient.Select.Columns("name", "surname").From("footable").AllRows();

			Assert.AreEqual("foo", res[0][0]);
			Assert.IsNull(res[0][1]);
		}

		[Test]
		public virtual void Can_select_all_rows() {
			CreateTableFoo();
			PopulateTableFoo();

			ResultSet res = _dataClient.Select
				.AllColumns()
				.From(tableFoo)
				.AllRows();

			Assert.AreEqual(3, res.Count);
			Assert.AreEqual(1, res[0][0]);
		}

		[Test]
		public virtual void Can_select_with_filter() {
			CreateTableFoo();
			PopulateTableFoo();

			ResultSet res = _dataClient.Select
				.AllColumns()
				.From(tableFoo)
				.Where(
				Filter.Eq("id", 1)
				).AllRows();

			Assert.AreEqual(1, res.Count);
			Assert.AreEqual(1, res[0][0]);
		}

		[Test]
		public void Can_select_with_pagination() {
			CreateTableFoo();
			PopulateTableFoo();

			ResultSet res = _dataClient.Select
									   .AllColumns()
									   .From(tableFoo)
									   .SkipTake(1, 1);
			
			Assert.AreEqual(1, res.Count);
			Assert.AreEqual(2, res[0][0]);
		}

		[Test]
		public void Can_select_with_pagination_and_where_filter() {
			CreateTableFoo();
			
			for (int i = 0; i < 10; i++) {
				_dataClient.Insert.Into(tableFoo).Columns("id", "name").Values(i, "sameValue");
			}
			for (int i = 10; i < 20; i++) {
				_dataClient.Insert.Into(tableFoo).Columns("id", "name").Values(i, "otherValue");
			}

			ResultSet res = _dataClient.Select
									   .AllColumns()
									   .From(tableFoo)
									   .Where(Filter.Eq("name", "sameValue"))
									   .SkipTake(5, 5);

			Assert.AreEqual(5, res.Count);
			Assert.AreEqual("sameValue", res[0][1]);
		}

		[Test]
		public virtual void Can_select_with_complex_filter() {
			CreateTableFoo();
			PopulateTableFoo();

			Filter complexFilter = CreateComplexFilter();

			ResultSet res = _dataClient.Select
				.AllColumns()
				.From(tableFoo)
				.Where(complexFilter)
				.AllRows();

			Assert.AreEqual(2, res.Count);
			Assert.AreEqual(1, res[0][0]);
			Assert.AreEqual(2, res[1][0]);
		}

		private static Filter CreateComplexFilter() {
			Filter idEqualsOne = Filter.Eq("id", 1);
			Filter nameEqualsV1 = Filter.Eq("name", "v1");
			Filter idEqualsTwo = Filter.Eq("id", 2);

			Filter filterAnd = Filter.And(idEqualsOne, nameEqualsV1);
			Filter filterOr = Filter.Or(filterAnd, idEqualsTwo);
			return filterOr;
		}

		[Test]
		public virtual void Can_update_rows() {
			CreateTableFoo();
			PopulateTableFoo();

			int num = _dataClient.Update
				.Table(tableFoo)
				.SetColumns("name")
				.ToValues("vvv")
				.AllRows();

			ResultSet res = _dataClient.Select.Columns("name").From(tableFoo).AllRows();

			Assert.AreEqual(3, num);
			Assert.AreEqual(3, res.Count);
			Assert.AreEqual("vvv", res[0][0]);
			Assert.AreEqual("vvv", res[1][0]);
			Assert.AreEqual("vvv", res[2][0]);
		}

		[Test]
		public virtual void Can_update_rows_with_filter() {
			CreateTableFoo();
			PopulateTableFoo();

			int num = _dataClient.Update
				.Table(tableFoo)
				.SetColumns("name")
				.ToValues("vvv")
				.Where(
					Filter.Or(Filter.Eq("id", 1), Filter.Eq("id", 2))
				);

			ResultSet res = _dataClient.Select.Columns("name").From(tableFoo).AllRows();

			Assert.AreEqual(2, num);
			Assert.AreEqual(3, res.Count);
			Assert.AreEqual("vvv", res[0][0]);
			Assert.AreEqual("vvv", res[1][0]);
			Assert.AreEqual("v3", res[2][0]);
		}

		[Test]
		public virtual void Can_delete_all_rows() {
			CreateTableFoo();
			PopulateTableFoo();

			int num = _dataClient.Delete.From(tableFoo).AllRows();

			ResultSet res = _dataClient.Select.AllColumns().From(tableFoo).AllRows();

			Assert.AreEqual(3, num);
			Assert.AreEqual(0, res.Count);
		}

		[Test]
		public virtual void Can_delete_rows_with_filter() {
			CreateTableFoo();
			PopulateTableFoo();

			Filter idEquals1 = Filter.Eq("id", 1);

			int num = _dataClient.Delete.From(tableFoo).Where(idEquals1);

			ResultSet res = _dataClient.Select.AllColumns().From(tableFoo).AllRows();

			Assert.AreEqual(1, num);
			Assert.AreEqual(2, res.Count);
		}

		[Test]
		public void Can_count() {
			CreateTableFoo();
			PopulateTableFoo();

			int num = _dataClient.Count.Table(tableFoo).AllRows();

			Assert.AreEqual(3, num);
		}

		[Test]
		public void Can_count_with_filter() {
			CreateTableFoo();
			PopulateTableFoo();

			int num = _dataClient.Count.Table(tableFoo).Where(Filter.Eq("id", 1));

			Assert.AreEqual(1, num);
		}

		[Test]
		public void Can_order_by_asc() {
			CreateTableFoo();
			PopulateTableFoo();
			_dataClient.Insert.Into(tableFoo).Columns("id", "name").Values(4, "aaa");

			ResultSet res = _dataClient.Select.AllColumns().From(tableFoo).OrderBy(OrderBy.Ascending("name")).AllRows();
			Assert.AreEqual("aaa", res[0]["name"]);
		}

		[Test]
		public void Can_order_by_desc() {
			CreateTableFoo();
			PopulateTableFoo();
			_dataClient.Insert.Into(tableFoo).Columns("id", "name").Values(4, "aaa");

			ResultSet res = _dataClient.Select.AllColumns().From(tableFoo).OrderBy(OrderBy.Descending("name")).AllRows();
			Assert.AreEqual("v3", res[0]["name"]);
		}

		[Test]
		public void Can_order_by_with_filter_and_pagination() {
			CreateTableFoo();
			PopulateTableFoo();
			_dataClient.Insert.Into(tableFoo).Columns("id", "name").Values(4, "aaa");

			ResultSet res = _dataClient.Select
										.AllColumns()
										.From(tableFoo)
										.Where(Filter.Gt("id", 1))
										.OrderBy(OrderBy.Ascending("name"))
										.SkipTake(1, 2);
			
			Assert.AreEqual("v2", res[0]["name"]);
		}

	}
}
