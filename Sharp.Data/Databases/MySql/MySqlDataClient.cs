namespace Sharp.Data.Databases.MySql {
	public class MySqlDataClient : DataClient {
		public MySqlDataClient(IDatabase database) : base(database, new MySqlDialect()) {}
	}
}