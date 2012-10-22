using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data.Databases.SqlServer;

namespace Sharp.Tests.Databases.SqlServer {
	public class SqlServerDialectTests  : DialectTests {

		[SetUp]
		public void SetUp() {
			_dialect = new SqlDialect();
		}

		protected override string GetResultFor_Can_create_check_if_table_exists_sql() {
			return "SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'mytable'";
		}

		protected override string[] GetResultFor_Can_create_table_sql() {
			return new string[2] {
				"create table mytable (id integer not null identity(1,1), name varchar(255) not null)",
				"alter table mytable add constraint pk_mytable primary key (id)"
			};
		}

		protected override string[] GetResultFor_Can_drop_table() {
			return new [] {"drop table myTable"};
		}

		protected override string GetResultFor_Can_convert_column_to_sql__with_not_null() {
			return "col varchar(255) not null";
		}

		protected override string GetResultFor_Can_convert_column_to_sql__with_primary_key() {
			return "col varchar(255) not null";
		}

		protected override string GetResultFor_Can_convert_column_to_sql__autoIncrement() {
			return "col integer not null identity(1,1)";
		}

		protected override string GetResultFor_Can_convert_column_to_sql__autoIncrement_and_primary_key() {
			return "col integer not null identity(1,1)";
		}

		protected override string GetResultFor_Can_convert_column_to_sql__default_value() {
			return "col varchar(255) null default ('some string')";
		}

		protected override string[] GetResultFor_Can_convert_column_to_values() {
			return new[] {
                     "'foo'",
                     "1",
                     "1",
                     "24.33",
                     "'2009-01-20T12:30:00'"
                };
		}

		protected override string GetResultFor_Can_generate_count_sql() {
            return "select count(*) from mytable";
		}

		protected override string GetResultFor_Can_generate_select_sql_with_pagination(int skip, int to) {
            string sql = @"select * into #TempTable from (
							select * ,ROW_NUMBER() over(order by aaa) AS rownum from (
								select 'aaa' as aaa, * from  (
									SELECT TOP 2147483647  * from myTable
								)as t1
							)as t2
						) as t3
					where rownum between {0} and {1}
					alter table #TempTable drop column aaa
					alter table #TempTable drop column rownum
					select * from #TempTable
					drop table #TempTable
				";
		    return String.Format(sql, skip + 1, skip + to);
		}
	}
}
