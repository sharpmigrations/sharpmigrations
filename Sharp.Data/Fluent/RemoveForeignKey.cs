using Sharp.Data;

namespace Sharp.Data.Fluent {
    public class RemoveForeignKey : RemoveItem {

        public RemoveForeignKey(IDataClient dataClient) : base(dataClient) {}

        protected override void ExecuteInternal() {
            DataClient.RemoveForeignKey(ItemName, _tableNames[0]);
        }
    }
}