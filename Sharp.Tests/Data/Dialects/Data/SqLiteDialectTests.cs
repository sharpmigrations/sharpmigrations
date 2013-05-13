using System;
using NUnit.Framework;
using Sharp.Data.Databases.SqLite;

namespace Sharp.Tests.Databases.SQLite {
	[TestFixture]
	public class SqLiteDialectDataTests : DialectDataTests {
		[SetUp]
		public void SetUp() {
			_dialect = new SqLiteDialect();
		}

		protected override string GetResultFor_Can_create_check_if_table_exists_sql() {
			return "SELECT count(name) FROM sqlite_master WHERE upper(name)=upper('" + TABLE_NAME + "')";
		}

		protected override string GetResultFor_Can_generate_count_sql() {
			return "SELECT COUNT(*) FROM myTable";
		}

		protected override string GetResultFor_Can_generate_select_sql_with_pagination(int skip, int to) {
			throw new NotImplementedException();
		}
	}
}