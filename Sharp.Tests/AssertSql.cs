using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Sharp.Tests {
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
			Assert.AreEqual(FormatSqlForTesting(sql1), FormatSqlForTesting(sql2));
		}

		public static void AreEqual(string[] sqls1, string[] sqls2) {
			Assert.AreEqual(sqls1.Length, sqls2.Length,
			                string.Format("Sizes doesn't match: {0} != {1}", sqls1.Length, sqls2.Length));
			for (int i = 0; i < sqls1.Length; i++) {
				AreEqual(sqls1[i], sqls2[i]);
			}
		}
	}
}