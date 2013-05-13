using System;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Databases.Oracle;

namespace Sharp.Tests.Databases.Oracle {
	[TestFixture]
	public class OracleDialectDataTests : DialectDataTests {
		[SetUp]
		public void SetUp() {
			_dialect = new OracleDialect();
		}

		protected override string GetResultFor_Can_create_check_if_table_exists_sql() {
			return "select count(table_name) from user_tables where upper(table_name) = upper('" + TABLE_NAME + "')";
		}

		protected override string GetResultFor_Can_generate_count_sql() {
			return "SELECT COUNT(*) FROM MYTABLE";
		}

		protected override string GetResultFor_Can_generate_select_sql_with_pagination(int skip, int take) {
			string sql = GetSelectAllSql();
			string innerSql = String.Format("select /* FIRST_ROWS(n) */ a.*, ROWNUM rnum from ({0}) a where ROWNUM <= {1}", sql, skip + take);
			return String.Format("select * from ({0}) where rnum > {1}", innerSql, skip);
		}
       
	}
}