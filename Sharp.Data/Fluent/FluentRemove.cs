namespace Sharp.Data.Fluent {
    
    public class FluentRemove {
        
        private IDataClient _dataClient;
        internal bool ThrowException { get; set; }

        public FluentRemove(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        public DateClientRemoveItem Column(string columnName) {
            RemoveColumn action = new RemoveColumn(_dataClient) { ItemName = columnName, ThrowException = ThrowException };
            return new DateClientRemoveItem(action);
        }

        public DateClientRemoveItem ForeignKey(string foreignKeyName) {
            RemoveForeignKey action = new RemoveForeignKey(_dataClient) { ItemName = foreignKeyName, ThrowException = ThrowException };
            return new DateClientRemoveItem(action);
        }

        public DateClientRemoveItem UniqueKey(string uniqueKeyName) {
            RemoveUniqueKey action = new RemoveUniqueKey(_dataClient) { ItemName = uniqueKeyName, ThrowException = ThrowException };
            return new DateClientRemoveItem(action);
        }

		public DateClientRemoveItem IndexKey(string IndexKeyName) {
			RemoveIndexKey action = new RemoveIndexKey(_dataClient) { ItemName = IndexKeyName, ThrowException = ThrowException };
			return new DateClientRemoveItem(action);
		}

        public void Table(string tableName) {
            RemoveTable action = new RemoveTable(_dataClient) { ThrowException = ThrowException };
            action.SetTableNames(tableName);
            action.Execute();
        }
    }
}