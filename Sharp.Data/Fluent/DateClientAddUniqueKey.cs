namespace Sharp.Data.Fluent {

	public class DataClientAddUniqueKey {
		private AddUniqueKey _action;

		public DataClientAddUniqueKey(AddUniqueKey action) {
			_action = action;
		}

		public DataClientAddUniqueKeyStep2 OnColumns(params string[] columnNames) {
			_action.ColumnNames = columnNames;
			return new DataClientAddUniqueKeyStep2(_action);
		}
	}

	public class DataClientAddUniqueKeyStep2 {
		private AddUniqueKey _action;

		public DataClientAddUniqueKeyStep2(AddUniqueKey action) {
			_action = action;
		}

		public void OfTable(string tableName) {
			_action.SetTableNames(tableName);
			_action.Execute();
		}
	}
}