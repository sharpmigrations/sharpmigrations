using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {
 
    public class FluentAdd {
    
        internal IDataClient _dataClient;

        public FluentAdd(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        public DataClientAddColumn Column(FluentColumn column) {
            var action = new AddColumn(_dataClient) { Column = column.Object };
            return new DataClientAddColumn(action);
        }

        public DataClientAddPrimaryKey PrimaryKey(string primaryKeyName) {
            var action = new AddPrimaryKey(_dataClient)
                                   {PrimaryKeyName = primaryKeyName};
            return new DataClientAddPrimaryKey(action);
        }

        public DataClientAddForeignKey ForeignKey(string foreignKeyName) {
            var action = new AddForeignKey(_dataClient) { ForeignKeyName = foreignKeyName};
            return new DataClientAddForeignKey(action);
        }

        public DataClientAddUniqueKey UniqueKey(string uniqueKeyName) {
            var action = new AddUniqueKey(_dataClient) { UniqueKeyName = uniqueKeyName};
            return new DataClientAddUniqueKey(action);
        }

		public DataClientAddIndexKey IndexKey(string indexKeyName) {
			var action = new AddIndexKey(_dataClient) { IndexKeyName = indexKeyName };
			return new DataClientAddIndexKey(action);
		}

        public DataClientAddTable Table(string tableName) {
            var action = new AddTable(_dataClient);
            action.SetTableNames(tableName);
            return new DataClientAddTable(action);
        }

        public IAddCommentColumnOrTable Comment(string comment) {
            var action = new AddComment(_dataClient, comment);
            return action;
        }
    }
}