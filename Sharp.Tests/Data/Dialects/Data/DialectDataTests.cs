using System;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Filters;
using Sharp.Data.Schema;
using Sharp.Tests.Data.Databases;

namespace Sharp.Tests.Databases {
	[Explicit]
	public abstract class DialectDataTests : DialectTests {

		protected abstract string GetResultFor_Can_create_check_if_table_exists_sql();
	    protected abstract string GetResultFor_Can_generate_count_sql();
	    
		protected virtual string GetInsertSql() {
			return String.Format("insert into foo (id, name) values ({0}par0,{0}par1)", _dialect.ParameterPrefix);
		}

		protected virtual string GetSelectAllSql() {
			return String.Format("select * from " + TABLE_NAME);			
		}

		protected abstract string GetResultFor_Can_generate_select_sql_with_pagination(int skip, int to);

		[Test]
		public void Can_create_check_if_table_exists_sql() {
			string sql = _dialect.GetTableExistsSql("myTable");
			AssertSql.AreEqual(GetResultFor_Can_create_check_if_table_exists_sql(), sql);
		}

		[Test]
		public void Can_generate_insert_sql() {
			string[] cols = {"id", "name"};
			object[] values = {1, "name1"};

			string sql = _dialect.GetInsertSql("foo", cols, values);

			AssertSql.AreEqual(GetInsertSql(), sql);
		}

		[Test]
		public void Can_generate_insert_sql_with_null_parameters() {
			string[] cols = {"id", "name"};
			object[] values = {1, null};

			string sql = _dialect.GetInsertSql("foo", cols, values);

			AssertSql.AreEqual(GetInsertSql(), sql);

			sql = _dialect.GetInsertSql("foo", cols, null);

			AssertSql.AreEqual(GetInsertSql(), sql);
		}

		[Test]
		public void Can_generate_select_all_sql() {
			string sql = _dialect.GetSelectSql(new[] { TABLE_NAME }, new[] { "*" });
			AssertSql.AreEqual(GetSelectAllSql(), sql);
		}

		[Test]
		public void Can_generate_select_sql_with_pagination() {
			string sql = _dialect.GetSelectSql(new[] { TABLE_NAME }, new[] { "*" });
			string sqlWithPagination = _dialect.WrapSelectSqlWithPagination(sql, 10, 20);
			AssertSql.AreEqual(GetResultFor_Can_generate_select_sql_with_pagination(10,20), sqlWithPagination);
		}

		[Test]
		public void Can_generate_count_sql() {
			string sql = _dialect.GetCountSql(TABLE_NAME);
			AssertSql.AreEqual(GetResultFor_Can_generate_count_sql(), sql);
		}

		

		[Test]
		public void Can_order_by_ascending_sql() {
			string sql = _dialect.GetOrderBySql(OrderBy.Ascending("col1"));
			AssertSql.AreEqual("order by col1", sql);			
		}

		[Test]
		public void Can_order_by_ascending_with_multiple_columns_sql() {
			string sql = _dialect.GetOrderBySql(OrderBy.Ascending("col1"), OrderBy.Ascending("col2"));
			AssertSql.AreEqual("order by col1, col2", sql);
		}

		[Test]
		public void Can_order_by_descending_sql() {
			string sql = _dialect.GetOrderBySql(OrderBy.Descending("col1"));
			AssertSql.AreEqual("order by col1 desc", sql);
		}

		[Test]
		public void Can_order_by_descending_with_multiple_columns_sql() {
			string sql = _dialect.GetOrderBySql(OrderBy.Descending("col1"), OrderBy.Descending("col2"));
			AssertSql.AreEqual("order by col1 desc, col2 desc", sql);
		}
		
		[Test]
		public void Can_order_by_ascending_and_descending_with_multiple_columns_sql() {
			string sql = _dialect.GetOrderBySql(OrderBy.Ascending("col1"), OrderBy.Descending("col2"));
			AssertSql.AreEqual("order by col1, col2 desc", sql);
		}

	    [Test]
        public void Can_select_with_multiple_tables_sql() {
            var tables = new string[] { "table1 t1", "table2 t2" };
            var columns = new string[] { "t1.col1", "t2.col2" };
            
            string sql = _dialect.GetSelectSql(tables, columns);
            AssertSql.AreEqual("select t1.col1 ,t2.col2 from table1 t1 ,table2 t2", sql);
	    }

        [Test]
        public void Can_select_with_multiple_tables_sql_2() {
            var tables = new string[] { "table1 t1", "table2 t2" };
            var columns = new string[] { "t1.col1", "t2.col1", "t2.col2" };

            string sql = _dialect.GetSelectSql(tables, columns);
            AssertSql.AreEqual("select t1.col1 ,t2.col1, t2.col2 from table1 t1 ,table2 t2", sql);
        }

	    [Test]
	    public void Can_convert_to_named_parameters() {
	        var objs = new object[] {"a", 1, DateTime.Today};
	        In[] pars = _dialect.ConvertToNamedParameters(objs);
            Assert.AreEqual(3, pars.Length);

	        for (int i = 0; i < 3; i++) {
                Assert.AreEqual(_dialect.ParameterPrefix + "par" + i, pars[i].Name);
                Assert.AreEqual(objs[i], pars[i].Value);    
	        }
	    }

        [Test]
        public void Convert_to_named_parameters__ignores_already_named_parameters() {
            object[] objs = new[] {In.Named(":par0", 1), In.Named(":par1", 2)};
            
            In[] pars = _dialect.ConvertToNamedParameters(objs);
            Assert.AreEqual(2, pars.Length);

            for (int i = 0; i < 2; i++) {
                Assert.AreEqual(objs[i], pars[i]);
            }
        }
	}
}