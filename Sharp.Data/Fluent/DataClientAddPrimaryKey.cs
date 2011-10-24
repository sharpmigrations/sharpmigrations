namespace Sharp.Data.Fluent {
    
    public class DataClientAddPrimaryKey {
        private AddPrimaryKey _action;

        public DataClientAddPrimaryKey(AddPrimaryKey action) {
            _action = action;
        }

        public DataClientAddPrimaryKeyStep2 OnColumns(params string[] columnNames) {
            _action.ColumnNames = columnNames;
            return new DataClientAddPrimaryKeyStep2(_action);
        }
    }

    public class DataClientAddPrimaryKeyStep2 {
        private AddPrimaryKey _action;

        public DataClientAddPrimaryKeyStep2(AddPrimaryKey action) {
            _action = action;
        }

        public void OfTable(string tableName) {
            _action.SetTableNames(tableName);
            _action.Execute();
        }
    }
}