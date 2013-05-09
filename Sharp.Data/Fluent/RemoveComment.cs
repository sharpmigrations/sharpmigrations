namespace Sharp.Data.Fluent {
    public class RemoveComment : DataClientAction, IRemoveCommentFromColumnOrTable, IRemoveCommentFromColumn {
        public string ColumnName { get; set; }

        public RemoveComment(IDataClient dataClient) : base(dataClient) {
        }

        public void FromTable(string tableName) {
            SetTableNames(tableName);
            Execute();
        }

        public IRemoveCommentFromColumn FromColumn(string column) {
            ColumnName = column;
            return this;
        }

        public void OfTable(string tableName) {
            SetTableNames(tableName);
            Execute();
        }

        protected override void ExecuteInternal() {
            if (ColumnName == null) {
                DataClient.RemoveTableComment(TableNames[0]);
                return;
            }
            DataClient.RemoveColumnComment(TableNames[0], ColumnName);
        }
    }

    public interface IRemoveCommentFromColumnOrTable {
        IRemoveCommentFromColumn FromColumn(string column);
        void FromTable(string tableName);
    }

    public interface IRemoveCommentFromColumn {
        void OfTable(string tableName);
    }
}