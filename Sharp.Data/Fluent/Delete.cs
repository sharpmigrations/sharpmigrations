using Sharp.Data.Filters;

namespace Sharp.Data.Fluent {
    public class Delete : DataClientAction {

        public int AfectedRows { get; set; }

        public Filter Filter { get; set; }

        public Delete(IDataClient dataClient) : base(dataClient) { }

        protected override void ExecuteInternal() {
            AfectedRows = DataClient.DeleteSql(TableNames[0], Filter);
        }
    }
}