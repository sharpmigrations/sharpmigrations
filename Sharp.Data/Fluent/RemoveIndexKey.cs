using Sharp.Data;

namespace Sharp.Data.Fluent {
    public class RemoveIndexKey : RemoveItem {
        public RemoveIndexKey(IDataClient dataClient) : base(dataClient) {}

        protected override void ExecuteInternal() {
            DataClient.RemoveIndex(ItemName, TableNames[0]);
        }
    }
}