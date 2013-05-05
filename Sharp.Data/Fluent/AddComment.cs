namespace Sharp.Data.Fluent {
    public class AddComment : DataClientAction, IAddCommentColumnOrTable, IAddCommentToColumn {
        public string Comment { get; set; }
        public string ColumnName { get; set; }

        public AddComment(IDataClient dataClient, string comment) : base(dataClient) {
            Comment = comment;
        }

        public IAddCommentToColumn ToColumn(string columnName) {
            ColumnName = columnName;
            return this;
        }

        public void ToTable(string tableName) {
            SetTableNames(tableName);
            Execute();
        }

        public void OfTable(string tableName) {
            SetTableNames(tableName);
            Execute();
        }

        protected override void ExecuteInternal() {
            if (ColumnName == null) {
                DataClient.AddTableComment(_tableNames[0], Comment);
                return;
            }
            DataClient.AddColumnComment(_tableNames[0], ColumnName, Comment);
        }
    }

    public interface IAddCommentColumnOrTable {
        void ToTable(string tableName);
        IAddCommentToColumn ToColumn(string columnName);
    }

    public interface IAddCommentToColumn {
        void OfTable(string tableName);
    }
}