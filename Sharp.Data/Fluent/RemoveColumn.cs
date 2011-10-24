using Sharp.Data;

namespace Sharp.Data.Fluent {
    public class RemoveColumn : RemoveItem {

        public RemoveColumn(IDataClient dataClient) : base(dataClient) {}

        protected override void ExecuteInternal() {
            DataClient.RemoveColumn(_tableNames[0], ItemName);
        }
    }
}