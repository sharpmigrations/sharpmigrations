using Sharp.Data;

namespace Sharp.Data.Fluent {
    public abstract class RemoveItem : DataClientAction {
        public string ItemName { get; set; }

        protected RemoveItem(IDataClient dataClient) : base(dataClient) {}
    }
}