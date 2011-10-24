using System;
namespace Sharp.Data.Fluent {
    public class Insert : DataClientAction {
        
        public string[] Columns { get; set; }
        public object[] Values { get; set; }
        public string ColumnToReturn { get; set; }
        public Type ColumnToReturnType { get; set; }
        public object ColumnToReturnValue { get; set; }

        public Insert(IDataClient dataClient) : base(dataClient) { }

        protected override void ExecuteInternal() {
            
            if(ColumnToReturn != null) {
                ColumnToReturnValue = DataClient.InsertReturningSql(_tableNames[0], ColumnToReturn, Columns, Values);
                return;
            }
            DataClient.InsertSql(_tableNames[0], Columns, Values);
        }
    }
}
