using System.Text.RegularExpressions;
using Xunit;

namespace SharpMigrator.Tests {
	public static class AssertSql {
		private static string FormatSqlForTesting(string value) {
			value = Regex.Replace(value, @"\s+", " ");
			return value.Trim()
				.Replace("\n", "")
				.Replace("\r", "")
				.Replace(", ", ",")
				.Replace(" ,", ",")
				.Replace("( ", "(")
				.Replace(" (", "(")
				.Replace(" )", ")")
				.Replace("  ", " ")
				.ToLower();
		}

		public static void AreEqual(string sql1, string sql2) {
			Assert.Equal(FormatSqlForTesting(sql1), FormatSqlForTesting(sql2));
		}

		public static void AreEqual(string[] sqls1, string[] sqls2) {
		    Assert.Equal(sqls1.Length, sqls2.Length);
			for (var i = 0; i < sqls1.Length; i++) {
				AreEqual(sqls1[i], sqls2[i]);
			}
		}
	}
}