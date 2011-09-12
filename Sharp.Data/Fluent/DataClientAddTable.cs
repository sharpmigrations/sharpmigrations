using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {
    public class DataClientAddTable {
        private AddTable _action;

        public DataClientAddTable(AddTable action) {
            _action = action;
        }

        public void WithColumns(params FluentColumn[] columns) {
            _action.Columns = columns;
            _action.Execute();
        }
    }
}