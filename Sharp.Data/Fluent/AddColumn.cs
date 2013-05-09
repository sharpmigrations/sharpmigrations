using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {
    public class AddColumn : DataClientAction, IAddColumnToTable {
        private Column _column;

        public AddColumn(IDataClient dataClient, Column column) : base(dataClient) {
            _column = column;
        }

        public void ToTable(string tableName) {
            SetTableNames(tableName);
            Execute();
        }

        protected override void ExecuteInternal() {
            DataClient.AddColumn(TableNames[0], _column);
        }

        public override DataClientAction ReverseAction() {
            return new RemoveColumn(DataClient) {
                FirstTableName = FirstTableName,
                ItemName = _column.ColumnName
            };
        }
    }

    public interface IAddColumnToTable {
        void ToTable(string tableName);
    }
}