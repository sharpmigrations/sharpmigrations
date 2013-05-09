using Sharp.Data;

namespace Sharp.Data.Fluent {
    public class AddPrimaryKey : DataClientAction, IAddPrimaryKeyOnColumns, IAddPrimaryKeyOfTable {
        
        public string PrimaryKeyName { get; set; }
        public string[] ColumnNames { get; set; }

        public AddPrimaryKey(IDataClient dataClient, string primaryKeyName) : base(dataClient) {
            PrimaryKeyName = primaryKeyName;
        }

        public IAddPrimaryKeyOfTable OnColumns(params string[] columnNames) {
            ColumnNames = columnNames;
            return this;
        }

        public void OfTable(string tableName) {
            SetTableNames(tableName);
            Execute();
        }

        protected override void ExecuteInternal() {
            DataClient.AddNamedPrimaryKey(TableNames[0], PrimaryKeyName, ColumnNames);
        }

        public override DataClientAction ReverseAction() {
            return new RemovePrimaryKey(DataClient, PrimaryKeyName) {
                FirstTableName = FirstTableName
            };
        }
    }

    public interface IAddPrimaryKeyOnColumns {
        IAddPrimaryKeyOfTable OnColumns(params string[] columnNames);
    }

    public interface IAddPrimaryKeyOfTable {
        void OfTable(string tableName);
    }
}
