namespace Sharp.Data.Fluent {
	public class DataClientAddIndexKey {
		private AddIndexKey _action;

		public DataClientAddIndexKey(AddIndexKey action) {
			_action = action;
		}

		public DataClientAddIndexKeyStep2 OnColumns(params string[] columnNames) {
			_action.ColumnNames = columnNames;
			return new DataClientAddIndexKeyStep2(_action);
		}
	}

	public class DataClientAddIndexKeyStep2 {
		private AddIndexKey _action;

		public DataClientAddIndexKeyStep2(AddIndexKey action) {
			_action = action;
		}

		public void OfTable(string tableName) {
			_action.SetTableNames(tableName);
			_action.Execute();
		}
	}
}