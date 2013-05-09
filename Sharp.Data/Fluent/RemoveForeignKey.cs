using Sharp.Data;

namespace Sharp.Data.Fluent {
    public class RemoveForeignKey : RemoveItemFromTable {

        public RemoveForeignKey(IDataClient dataClient, string foreignKeyName) : base(dataClient) {
            ItemName = foreignKeyName;
        }

        protected override void ExecuteInternal() {
            DataClient.RemoveForeignKey(ItemName, TableNames[0]);
        }
    }
}