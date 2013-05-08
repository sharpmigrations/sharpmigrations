namespace Sharp.Data.Fluent {
    public class RenameColumn : DataClientAction, IRenameColumnOfTable, IRenameColumnTo {
        private string _newName, _columnName;

        public RenameColumn(IDataClient dataClient, string columnName)
            : base(dataClient) {
            _columnName = columnName;
        }

        public IRenameColumnTo OfTable(string tableName) {
            SetTableNames(tableName);
            return this;
        }

        public void To(string newName) {
            _newName = newName;
            Execute();
        }

        protected override void ExecuteInternal() {
            DataClient.RenameColumn(_tableNames[0], _columnName, _newName);
        }
    }

    public interface IRenameColumnOfTable {
        IRenameColumnTo OfTable(string tableName);
    }

    public interface IRenameColumnTo {
        void To(string newName);
    }
}