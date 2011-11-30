using System;

namespace Sharp.Data.Databases.SqlServer {
	public class SqlServerDataClient : DataClient {
		private Dialect _dialect;
		public SqlServerDataClient(IDatabase database) : base(database) {}

		public override Dialect Dialect {
			get {
				if (_dialect == null) {
					_dialect = new SqlDialect();
				}
				return _dialect;
			}
			set { _dialect = value; }
		}

		public override void RemoveColumn(string tableName, string columnName) {
			string[] sqls = Dialect.GetDropColumnSql(tableName, columnName);
			object defaultConstraintName = Database.QueryScalar(sqls[0]);
			if (defaultConstraintName != null) {
				Database.ExecuteSql(String.Format(sqls[1], defaultConstraintName));
			}
			Database.ExecuteSql(sqls[2]);
		}
	}
}