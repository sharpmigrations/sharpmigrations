using Sharp.Data;
using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {

    public class AddTable : DataClientAction {
        
        public FluentColumn[] Columns { get; set; }

        public AddTable(IDataClient dataClient) : base(dataClient) { }

        protected override void ExecuteInternal() {
            DataClient.AddTable(TableName, Columns);
        }
    }
}