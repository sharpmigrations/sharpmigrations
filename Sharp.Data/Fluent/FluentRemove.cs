namespace Sharp.Data.Fluent {
    
    public class FluentRemove {
        
        private IDataClient _dataClient;
        internal bool ThrowException { get; set; }

        public FluentRemove(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        public DateClientRemoveItem Column(string columnName) {
            var action = new RemoveColumn(_dataClient) { ItemName = columnName };
            return new DateClientRemoveItem(action);
        }

        public IRemoveCommentFromColumnOrTable Comment {
            get { return new RemoveComment(_dataClient); }
        }

        public IRemovePrimaryKeyOfTable PrimaryKey(string primaryKeyName) {
            return new RemovePrimaryKey(_dataClient, primaryKeyName);
        }

        public DateClientRemoveItem ForeignKey(string foreignKeyName) {
            var action = new RemoveForeignKey(_dataClient) { ItemName = foreignKeyName};
            return new DateClientRemoveItem(action);
        }

        public DateClientRemoveItem UniqueKey(string uniqueKeyName) {
            var action = new RemoveUniqueKey(_dataClient) { ItemName = uniqueKeyName };
            return new DateClientRemoveItem(action);
        }

        public DateClientRemoveItem IndexKey(string indexKeyName) {
            var action = new RemoveIndexKey(_dataClient) { ItemName = indexKeyName };
			return new DateClientRemoveItem(action);
		}

        public void Table(string tableName) {
            var action = new RemoveTable(_dataClient);
            action.SetTableNames(tableName);
            action.Execute();
        }
    }
}