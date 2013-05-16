namespace Sharp.Data.Fluent {
    
    public class AddIndexKey : DataClientAction {
        
        public string IndexKeyName { get; set; }
        public string[] ColumnNames { get; set; }

		public AddIndexKey(IDataClient dataClient) : base(dataClient) { }

        protected override void ExecuteInternal() {
            DataClient.AddIndex(IndexKeyName, TableNames[0], ColumnNames);
        }

        public override DataClientAction ReverseAction() {
            return new RemoveIndexKey(DataClient, IndexKeyName) {
                FirstTableName = FirstTableName
            };
        }
    }
}