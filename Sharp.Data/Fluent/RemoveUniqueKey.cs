using Sharp.Data;

namespace Sharp.Data.Fluent {
    public class RemoveUniqueKey : RemoveItem {
        public RemoveUniqueKey(IDataClient dataClient) : base(dataClient) {}

        protected override void ExecuteInternal() {
            DataClient.RemoveUniqueKey(ItemName, TableNames[0]);
        }
    }
}