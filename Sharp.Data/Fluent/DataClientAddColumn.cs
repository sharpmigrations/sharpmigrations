using Sharp.Data.Fluent;

namespace Sharp.Data.Fluent {

    public class DataClientAddColumn {
        
        private AddColumn _action;

        public DataClientAddColumn(AddColumn action) {
            _action = action;
        }

        public void ToTable(string tableName) {
            _action.SetTableNames(tableName);
            _action.Execute();
        }
    }
}