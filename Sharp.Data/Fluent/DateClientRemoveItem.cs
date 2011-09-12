namespace Sharp.Data.Fluent {
    public class DateClientRemoveItem {
        private RemoveItem _action;

        public DateClientRemoveItem(RemoveItem action) {
            _action = action;
        }

        public void FromTable(string tableName) {
            _action.TableName = tableName;
            _action.Execute();
        }
    }
}