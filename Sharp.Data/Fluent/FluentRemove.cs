namespace Sharp.Data.Fluent {
    
    public class FluentRemove {
        
        private IDataClient _dataClient;

        public FluentRemove(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        public IRemoveFromTable Column(string columnName) {
            return new RemoveColumn(_dataClient) { ItemName = columnName };
        }

        public IRemoveCommentFromColumnOrTable Comment {
            get { return new RemoveComment(_dataClient); }
        }

        public IRemoveFromTable PrimaryKey(string primaryKeyName) {
            return new RemovePrimaryKey(_dataClient, primaryKeyName);
        }

        public IRemoveFromTable ForeignKey(string foreignKeyName) {
            return new RemoveForeignKey(_dataClient, foreignKeyName);
        }

        public IRemoveFromTable UniqueKey(string uniqueKeyName) {
            return new RemoveUniqueKey(_dataClient, uniqueKeyName) { ItemName = uniqueKeyName };
        }

        public IRemoveFromTable IndexKey(string indexKeyName) {
            return new RemoveIndexKey(_dataClient, indexKeyName) { ItemName = indexKeyName };
		}

        public void Table(string tableName) {
            var action = new RemoveTable(_dataClient);
            action.SetTableNames(tableName);
            action.Execute();
        }
    }
}