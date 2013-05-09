using Sharp.Data;

namespace Sharp.Data.Fluent {
    public class RemoveUniqueKey : RemoveItemFromTable {
        public RemoveUniqueKey(IDataClient dataClient, string uniqueKeyName) : base(dataClient) {
            ItemName = uniqueKeyName;
        }

        public void FromTable(string tableName) {
            SetTableNames(tableName);
            Execute();
        }

        protected override void ExecuteInternal() {
            DataClient.RemoveUniqueKey(ItemName, TableNames[0]);
        }
    }
}