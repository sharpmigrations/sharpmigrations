using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {
    
    public class AddColumn : DataClientAction {

        public AddColumn(IDataClient dataClient) : base(dataClient) {}

        public Column Column { get; set; }

        protected override void ExecuteInternal() {
            DataClient.AddColumn(TableName, Column);
        }
    }
}