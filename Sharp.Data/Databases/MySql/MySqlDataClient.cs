namespace Sharp.Data.Databases.MySql {
	public class MySqlDataClient : DataClient {
		private Dialect _dialect;
		public MySqlDataClient(IDatabase database) : base(database) {}

		public override Dialect Dialect {
			get {
				if (_dialect == null) {
					_dialect = new MySqlDialect();
				}
				return _dialect;
			}
			set { _dialect = value; }
		}
	}
}