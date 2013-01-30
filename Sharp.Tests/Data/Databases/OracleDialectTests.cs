using System;
using NUnit.Framework;
using Sharp.Data;

namespace Sharp.Tests.Databases.Oracle {
	[TestFixture]
	public class OracleDialectTests : DialectTests {
		[SetUp]
		public void SetUp() {
			_dialect = new OracleDialect();
		}

		protected override string GetResultFor_Can_create_check_if_table_exists_sql() {
			return "select count(table_name) from user_tables where upper(table_name) = upper('" + TABLE_NAME + "')";
		}

		protected override string[] GetResultFor_Can_create_table_sql() {
			return new [] {
				"create table myTable (id number(10) not null, name varchar2(255) not null)",
				"create sequence seq_mytable minvalue 1 maxvalue 999999999999999999999999999 start with 1 increment by 1 cache 20",
				"create or replace trigger \"tr_inc_mytable\" before insert on mytable for each row when (new.id is null) begin select seq_mytable.nextval into :new.id from dual; end tr_inc_mytable;",
				"alter table mytable add constraint pk_mytable primary key (id)"
			};
		}

		protected override string[] GetResultFor_Can_drop_table() {
			return new [] { "drop table mytable cascade constraints", 
				            "begin execute immediate 'drop sequence seq_mytable'; exception when others then null; end;" };
		}

		protected override string GetResultFor_Can_convert_column_to_sql__with_not_null() {
			return "col varchar2(255) not null";
		}

		protected override string GetResultFor_Can_convert_column_to_sql__with_primary_key() {
			return "col varchar2(255) not null";
		}

		protected override string GetResultFor_Can_convert_column_to_sql__autoIncrement() {
			return "col number(10) not null";
		}

		protected override string GetResultFor_Can_convert_column_to_sql__autoIncrement_and_primary_key() {
			return "col number(10) not null";
		}

		protected override string GetResultFor_Can_convert_column_to_sql__default_value() {
			return "col varchar2(255) default 'some string' null";
		}

		protected override string[] GetResultFor_Can_convert_column_to_values() {
			return new[] {
				"'foo'",
				"1",
				"1",
				"24.33",
				"to_date('20/1/2009 12:30:0','dd/mm/yyyy hh24:mi:ss')"
			};
		}

		protected override string GetResultFor_Can_generate_count_sql() {
			return "SELECT COUNT(*) FROM MYTABLE";
		}

		protected override string GetResultFor_Can_generate_select_sql_with_pagination(int skip, int take) {
			string sql = GetSelectAllSql();
			string innerSql = String.Format("select /* FIRST_ROWS(n) */ a.*, ROWNUM rnum from ({0}) a where ROWNUM <= {1}", sql, skip + take);
			return String.Format("select * from ({0}) where rnum > {1}", innerSql, skip);
		}

        protected override string GetResutFor_Can_drop_index_sql() {
            return "drop index indexName";
        }
	}
}