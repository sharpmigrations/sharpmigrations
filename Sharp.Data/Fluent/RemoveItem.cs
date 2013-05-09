namespace Sharp.Data.Fluent {
    public abstract class RemoveItemFromTable : DataClientAction, IRemoveFromTable {
        public string ItemName { get; set; }
        protected RemoveItemFromTable(IDataClient dataClient) : base(dataClient) {}

        public void FromTable(string tableName) {
            SetTableNames(tableName);
            Execute();
        }
    }
}