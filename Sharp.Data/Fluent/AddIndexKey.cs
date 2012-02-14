namespace Sharp.Data.Fluent {
    
    public class AddIndexKey : DataClientAction {
        
        public string IndexKeyName { get; set; }
        public string[] ColumnNames { get; set; }

		public AddIndexKey(IDataClient dataClient) : base(dataClient) { }

        protected override void ExecuteInternal() {
            DataClient.AddIndex(IndexKeyName, _tableNames[0], ColumnNames);
        }
    }
}