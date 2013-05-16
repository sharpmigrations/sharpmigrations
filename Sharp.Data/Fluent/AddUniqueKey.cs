namespace Sharp.Data.Fluent {
   
    public class AddUniqueKey : DataClientAction, IAddUniqueKeyOnColumns, IAddUniqueKeyOfTable {
        
        public string UniqueKeyName { get; set; }
        public string[] ColumnNames { get; set; }

        public AddUniqueKey(IDataClient dataClient, string uniqueKeyName) : base(dataClient) {
            UniqueKeyName = uniqueKeyName;
        }

        public IAddUniqueKeyOfTable OnColumns(params string[] columnNames) {
            ColumnNames = columnNames;
            return this;
        }

        public void OfTable(string tableName) {
            FirstTableName = tableName;
            Execute();
        }

        protected override void ExecuteInternal() {
            DataClient.AddUniqueKey(UniqueKeyName, TableNames[0], ColumnNames);
        }

        public override DataClientAction ReverseAction() {
            return new RemoveUniqueKey(DataClient, UniqueKeyName) {
                FirstTableName = FirstTableName
            };
        }
    }

    public interface IAddUniqueKeyOnColumns {
        IAddUniqueKeyOfTable OnColumns(params string[] columnNames);
    }

    public interface IAddUniqueKeyOfTable {
        void OfTable(string tableName);
    }
}