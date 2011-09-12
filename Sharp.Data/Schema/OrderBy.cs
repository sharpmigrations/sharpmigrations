namespace Sharp.Data.Schema {
	public class OrderBy {
		public string ColumnName { get; set; }
		public OrderByDirection Direction { get; set; }

		public static OrderBy Ascending(string columnName) {
			return new OrderBy {ColumnName = columnName, Direction = OrderByDirection.Ascending };
		}

		public static OrderBy Descending(string  columnName) {
			return new OrderBy { ColumnName = columnName, Direction = OrderByDirection.Descending };			
		}
	}
}