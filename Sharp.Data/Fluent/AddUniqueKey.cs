namespace Sharp.Data.Fluent {
    
    public class AddUniqueKey : DataClientAction {
        
        public string UniqueKeyName { get; set; }
        public string[] ColumnNames { get; set; }

        public AddUniqueKey(IDataClient dataClient) : base(dataClient) { }

        protected override void ExecuteInternal() {
            DataClient.AddUniqueKey(UniqueKeyName, TableNames[0], ColumnNames);
        }
    }
}