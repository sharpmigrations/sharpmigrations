using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {
    public class ModifyColumn : DataClientAction {
        private string _columnName;
        private FluentColumn _columnDefinition;

        public ModifyColumn(IDataClient dataClient, string columnName) 
            : base(dataClient) {
            _columnName = columnName;
        }

        public ModifyColumn OfTable(string tableName) {
            SetTableNames(tableName);
            return this;
        }

        public void WithDefinition(FluentColumn columnDef) {
            _columnDefinition = columnDef;
            Execute();
        }

        protected override void ExecuteInternal() {
            DataClient.ModifyColumn(FirstTableName, _columnName, _columnDefinition.Object);
        }
    }
}