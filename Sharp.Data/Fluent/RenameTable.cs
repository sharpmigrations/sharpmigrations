namespace Sharp.Data.Fluent {
    public class RenameTable : DataClientAction, IRenameTableTo {
        private string _newName;

        public RenameTable(IDataClient dataClient, string tableName) : base(dataClient) {
            SetTableNames(tableName);
        }

        public void To(string newName) {
            _newName = newName;
            Execute();
        }

        protected override void ExecuteInternal() {
            DataClient.RenameTable(TableNames[0], _newName);
        }
    }

    public interface IRenameTableTo {
        void To(string newName);
    }
}