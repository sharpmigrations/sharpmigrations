using System;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Filters;
using Sharp.Data.Schema;

namespace Sharp.Tests.Databases {
	[Explicit]
	public abstract class DialectTests {
		protected string TABLE_NAME = "myTable";

		protected Dialect _dialect;
		protected abstract string GetResultFor_Can_create_check_if_table_exists_sql();
		protected abstract string[] GetResultFor_Can_create_table_sql();
		protected abstract string[] GetResultFor_Can_drop_table();
		protected abstract string GetResultFor_Can_convert_column_to_sql__with_not_null();
		protected abstract string GetResultFor_Can_convert_column_to_sql__with_primary_key();
		protected abstract string GetResultFor_Can_convert_column_to_sql__autoIncrement();
		protected abstract string GetResultFor_Can_convert_column_to_sql__autoIncrement_and_primary_key();
		protected abstract string GetResultFor_Can_convert_column_to_sql__default_value();
		protected abstract string[] GetResultFor_Can_convert_column_to_values();
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
		public void Can_create_table_sql() {
			Table table = new Table("myTable");
			table.Columns.Add(Column.AutoIncrement("id").AsPrimaryKey().Object);
			table.Columns.Add(Column.String("name").NotNull().Object);

			string[] sqls = _dialect.GetCreateTableSqls(table);
			AssertSql.AreEqual(GetResultFor_Can_create_table_sql(), sqls);
		}

		[Test]
		public void Can_drop_table() {
			string[] sqls = _dialect.GetDropTableSqls("myTable");
			AssertSql.AreEqual(GetResultFor_Can_drop_table(), sqls);
		}

		[Test]
		public void Can_convert_column_to_sql__with_not_null() {
			Column column = Column.String("col")
				.Size(255)
				.NotNull().Object;

			string sql = _dialect.GetColumnToSqlWhenCreate(column);

			AssertSql.AreEqual(GetResultFor_Can_convert_column_to_sql__with_not_null(), sql);
		}

		[Test]
		public void Can_convert_column_to_sql__with_primary_key() {
			Column column = Column.String("col")
				.Size(255)
				.AsPrimaryKey()
				.Object;

			string sql = _dialect.GetColumnToSqlWhenCreate(column);

			AssertSql.AreEqual(GetResultFor_Can_convert_column_to_sql__with_primary_key(), sql);
		}

		[Test]
		public virtual void Can_convert_column_to_sql__autoIncrement() {
			Column column = Column.AutoIncrement("col").Object;

			string sql = _dialect.GetColumnToSqlWhenCreate(column);

			AssertSql.AreEqual(GetResultFor_Can_convert_column_to_sql__autoIncrement(), sql);
		}

		[Test]
		public virtual void Can_convert_column_to_sql__default_value() {
			Column column = Column.String("col")
								  .DefaultValue("some string").Object;

			string sql = _dialect.GetColumnToSqlWhenCreate(column);

			AssertSql.AreEqual(GetResultFor_Can_convert_column_to_sql__default_value(), sql.ToUpper());
		}

		[Test]
		public virtual void Can_convert_column_to_sql__autoIncrement_and_primary_key() {
			Column column = Column.AutoIncrement("col").AsPrimaryKey().Object;

			string sql = _dialect.GetColumnToSqlWhenCreate(column);

			AssertSql.AreEqual(GetResultFor_Can_convert_column_to_sql__autoIncrement_and_primary_key(), sql);
		}

		[Test]
		public virtual void Can_convert_column_to_values() {
			Assert.AreEqual(GetResultFor_Can_convert_column_to_values()[0], _dialect.GetColumnValueToSql("foo"));
			Assert.AreEqual(GetResultFor_Can_convert_column_to_values()[1], _dialect.GetColumnValueToSql(1));
			Assert.AreEqual(GetResultFor_Can_convert_column_to_values()[2], _dialect.GetColumnValueToSql(true));
			Assert.AreEqual(GetResultFor_Can_convert_column_to_values()[3], _dialect.GetColumnValueToSql(24.33));
			Assert.AreEqual(GetResultFor_Can_convert_column_to_values()[4],
			                _dialect.GetColumnValueToSql(new DateTime(2009, 1, 20, 12, 30, 0, 567)));
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
		public void Can_create_index_sql() {
			string sql = _dialect.GetCreateIndexSql("index", TABLE_NAME, "col1");
			AssertSql.AreEqual("create index index on myTable (col1)", sql);
		}

		[Test]
		public void Can_create_index_with_multiple_columns_sql() {
			string sql = _dialect.GetCreateIndexSql("indexName", TABLE_NAME, "col1", "col2", "col3");
			AssertSql.AreEqual("create index indexName on myTable (col1, col2, col3)", sql);
		}

		[Test]
		public void Can_drop_index_sql() {
			string sql = _dialect.GetDropIndexSql("indexName");
			AssertSql.AreEqual("drop index indexName", sql);
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
            string[] tables = new string[] { "table1 t1", "table2 t2" };
            string[] columns = new string[] { "t1.col1", "t2.col2" };
            
            string sql = _dialect.GetSelectSql(tables, columns);
            AssertSql.AreEqual("select t1.col1 ,t2.col2 from table1 t1 ,table2 t2", sql);
	    }

        [Test]
        public void Can_select_with_multiple_tables_sql_2() {
            string[] tables = new string[] { "table1 t1", "table2 t2" };
            string[] columns = new string[] { "t1.col1", "t2.col1", "t2.col2" };

            string sql = _dialect.GetSelectSql(tables, columns);
            AssertSql.AreEqual("select t1.col1 ,t2.col1, t2.col2 from table1 t1 ,table2 t2", sql);
        }
	}
}