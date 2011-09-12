using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {

    public class DataClientAddForeignKey {
        private AddForeignKey _action;

        public DataClientAddForeignKey(AddForeignKey action) {
            _action = action;
        }

        public DataClientAddForeignKeyStep2 OnColumn(string columnName) {
            _action.Column = columnName;
            return new DataClientAddForeignKeyStep2(_action);
        }
    }

    public class DataClientAddForeignKeyStep2 {
        private AddForeignKey _action;

        public DataClientAddForeignKeyStep2(AddForeignKey action) {
            _action = action;
        }

        public DataClientAddForeignKeyStep3 OfTable(string tableName) {
            _action.TableName = tableName;
            return new DataClientAddForeignKeyStep3(_action);
        }
    }

    public class DataClientAddForeignKeyStep3 {
        private AddForeignKey _action;

        public DataClientAddForeignKeyStep3(AddForeignKey action) {
            _action = action;
        }

        public DataClientAddForeignKeyStep4 ReferencingColumn(string columnName) {
            _action.ReferencingColumn = columnName;
            return new DataClientAddForeignKeyStep4(_action);
        }
    }

    public class DataClientAddForeignKeyStep4 {
        private AddForeignKey _action;

        public DataClientAddForeignKeyStep4(AddForeignKey action) {
            _action = action;
        }

        public DataClientAddForeignKeyStep5 OfTable(string referencingTable) {
            _action.ReferencingTable = referencingTable;
            return new DataClientAddForeignKeyStep5(_action);
        }
    }

    public class DataClientAddForeignKeyStep5 {
        private AddForeignKey _action;

        public DataClientAddForeignKeyStep5(AddForeignKey action) {
            _action = action;
        }

        public void OnDeleteSetNull() {
            _action.OnDelete = OnDelete.SetNull;
            _action.Execute();
        }

        public void OnDeleteNoAction() {
            _action.OnDelete = OnDelete.NoAction;
            _action.Execute();
        }

        public void OnDeleteCascade() {
            _action.OnDelete = OnDelete.Cascade;
            _action.Execute();
        }
    }
}