using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {
    public class FluentAdd : ReversableFluentActions, IFluentAdd {
    
        private IDataClient _dataClient;
        
        public FluentAdd(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        public IAddColumnToTable Column(FluentColumn column) {
            var action = new AddColumn(_dataClient, column.Object);
            FireOnAction(action);
            return action;
        }

        public IAddPrimaryKeyOnColumns PrimaryKey(string primaryKeyName) {
            var action = new AddPrimaryKey(_dataClient, primaryKeyName);
            FireOnAction(action);
            return action;
        }

        public IAddForeignKeyOnColumn ForeignKey(string foreignKeyName) {
            var action = new AddForeignKey(_dataClient, foreignKeyName);
            FireOnAction(action);
            return action;
        }

        public IAddUniqueKeyOnColumns UniqueKey(string uniqueKeyName) {
            var action = new AddUniqueKey(_dataClient, uniqueKeyName);
            FireOnAction(action);
            return action;
        }

		public DataClientAddIndexKey IndexKey(string indexKeyName) {
			var action = new AddIndexKey(_dataClient) { IndexKeyName = indexKeyName };
			return new DataClientAddIndexKey(action);
		}

        public IAddTableWithColumns Table(string tableName) {
            var action = new AddTable(_dataClient, tableName);
            FireOnAction(action);
            return action;
        }

        public IAddCommentColumnOrTable Comment(string comment) {
            var action = new AddComment(_dataClient, comment);
            FireOnAction(action);
            return action;
        }
    }
}