using Sharp.Data;
using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {

    public class AddForeignKey : DataClientAction, IAddForeignKeyOnColumn, IAddForeignKeyOfTable, IAddForeignKeyReferencingColumn, IAddForeignKeyOnDelete {
        
        public string ForeignKeyName { get; set; }
        public string Column { get; set; }
        public string ReferencingColumnName { get; set; }
        public string ReferencingTable { get; set; }
        public OnDelete OnDelete { get; set; }

        public AddForeignKey(IDataClient dataClient, string fkName) : base(dataClient) {
            ForeignKeyName = fkName;
        }

        public IAddForeignKeyOfTable OnColumn(string columnName) {
            Column = columnName;
            return this;
        }

        public IAddForeignKeyReferencingColumn OfTable(string tableName) {
            FirstTableName = tableName;
            return this;
        }

        public AddForeignKeyStepOfTable ReferencingColumn(string columnName) {
            ReferencingColumnName = columnName;
            return new AddForeignKeyStepOfTable(this);
        }

        public void OnDeleteSetNull() {
            OnDelete = OnDelete.SetNull;
            Execute();
        }

        public void OnDeleteNoAction() {
            OnDelete = OnDelete.NoAction;
            Execute();
        }

        public void OnDeleteCascade() {
            OnDelete = OnDelete.Cascade;
            Execute();
        }

        protected override void ExecuteInternal() {
            DataClient.AddForeignKey(ForeignKeyName, TableNames[0], Column, ReferencingTable, ReferencingColumnName, OnDelete);
        }

        public override DataClientAction ReverseAction() {
            return new RemoveForeignKey(DataClient, ForeignKeyName) {
                FirstTableName = FirstTableName
            };
        }
    }

    public interface IAddForeignKeyOnColumn {
        IAddForeignKeyOfTable OnColumn(string columnName);
    }

    public interface IAddForeignKeyOfTable {
        IAddForeignKeyReferencingColumn OfTable(string tableName);
    }

    public interface IAddForeignKeyReferencingColumn {
        AddForeignKeyStepOfTable ReferencingColumn(string columnName);
    }

    public interface IAddForeignKeyOnDelete {
        void OnDeleteSetNull();
        void OnDeleteNoAction();
        void OnDeleteCascade();
    }

    public class AddForeignKeyStepOfTable {
        private AddForeignKey _addForeign;
        public AddForeignKeyStepOfTable(AddForeignKey addForeign) {
            _addForeign = addForeign;
        }

        public IAddForeignKeyOnDelete OfTable(string referencingTable) {
            _addForeign.ReferencingTable = referencingTable;
            return _addForeign;
        }
    }
}