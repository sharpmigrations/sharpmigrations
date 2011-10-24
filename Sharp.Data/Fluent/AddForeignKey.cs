using Sharp.Data;
using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {
    
    public class AddForeignKey : DataClientAction {
        
        public string ForeignKeyName { get; set; }
        public string Column { get; set; }
        public string Table { get; set; }
        public string ReferencingColumn { get; set; }
        public string ReferencingTable { get; set; }
        public OnDelete OnDelete { get; set; }

        public AddForeignKey(IDataClient dataClient) : base(dataClient) {}

        protected override void ExecuteInternal() {
            DataClient.AddForeignKey(ForeignKeyName, _tableNames[0], Column, ReferencingTable, ReferencingColumn, OnDelete);
        }
    }
}