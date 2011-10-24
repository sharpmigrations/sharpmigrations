using Sharp.Data;

namespace Sharp.Data.Fluent {
    public class AddPrimaryKey : DataClientAction {
        
        public string PrimaryKeyName { get; set; }
        public string[] ColumnNames { get; set; }

        public AddPrimaryKey(IDataClient dataClient) : base(dataClient) { }

        protected override void ExecuteInternal() {
            DataClient.AddNamedPrimaryKey(PrimaryKeyName, _tableNames[0], ColumnNames);
        }
    }
}
