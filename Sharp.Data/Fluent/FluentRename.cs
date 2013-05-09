namespace Sharp.Data.Fluent {
    public class FluentRename : ReversableFluentActions, IFluentRename {

        private IDataClient _dataClient;

        public FluentRename(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        public IRenameTableTo Table(string tableName) {
            var action = new RenameTable(_dataClient, tableName);
            FireOnAction(action);
            return action;
        }

        public IRenameColumnOfTable Column(string columnName) {
            var action = new RenameColumn(_dataClient, columnName);
            FireOnAction(action);
            return action;
        }
    }
}
