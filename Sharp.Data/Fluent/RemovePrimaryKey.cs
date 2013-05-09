namespace Sharp.Data.Fluent {
    public class RemovePrimaryKey : RemoveItemFromTable {
        public RemovePrimaryKey(IDataClient dataClient, string primaryKeyName) : base(dataClient) {
            ItemName = primaryKeyName;
        }

        protected override void ExecuteInternal() {
            DataClient.RemovePrimaryKey(FirstTableName, ItemName);
        }
    }
}
