using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {
    
    public class AddTable : DataClientAction, IAddTableWithColumns {

        private FluentColumn[] _columns;
        
        public AddTable(IDataClient dataClient, string tableName) : base(dataClient) {
            SetTableNames(tableName);
        }
        
        public void WithColumns(params FluentColumn[] columns) {
            _columns = columns;
            Execute();
        }

        protected override void ExecuteInternal() {
            DataClient.AddTable(TableNames[0], _columns);
        }

        public override DataClientAction ReverseAction() {
            return new RemoveTable(DataClient) {
                FirstTableName = FirstTableName
            };
        }
    }
    
    public interface IAddTableWithColumns {
        void WithColumns(params FluentColumn[] columns);
    }
}