using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {
 
    public class FluentAdd {
    
        internal IDataClient _dataClient;

        public FluentAdd(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        public DataClientAddColumn Column(FluentColumn column) {
            AddColumn action = new AddColumn(_dataClient) { Column = column.Object };
            return new DataClientAddColumn(action);
        }

        public DataClientAddPrimaryKey PrimaryKey(string primaryKeyName) {
            AddPrimaryKey action = new AddPrimaryKey(_dataClient)
                                   {PrimaryKeyName = primaryKeyName};
            return new DataClientAddPrimaryKey(action);
        }

        public DataClientAddForeignKey ForeignKey(string foreignKeyName) {
            AddForeignKey action = new AddForeignKey(_dataClient) { ForeignKeyName = foreignKeyName};
            return new DataClientAddForeignKey(action);
        }

        public DataClientAddUniqueKey UniqueKey(string uniqueKeyName) {
            AddUniqueKey action = new AddUniqueKey(_dataClient) { UniqueKeyName = uniqueKeyName};
            return new DataClientAddUniqueKey(action);
        }

        public DataClientAddTable Table(string tableName) {
            AddTable action = new AddTable(_dataClient) { TableName = tableName};
            return new DataClientAddTable(action);
        }
    }
}