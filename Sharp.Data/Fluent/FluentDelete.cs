using Sharp.Data.Filters;

namespace Sharp.Data.Fluent {
	public class FluentDelete : IFluentDelete, IFluentDeleteFilter {

        private Delete _delete;
        
        public FluentDelete(IDataClient client) {
            _delete = new Delete(client);
        }

        public IFluentDeleteFilter From(string tableName) {
            _delete.SetTableNames(tableName);
        	return this;
        }

		public int AllRows() {
			_delete.Execute();
			return _delete.AfectedRows;
		}

		public int Where(Filter where) {
			_delete.Filter = where;
			_delete.Execute();
			return _delete.AfectedRows;
		}
    }

	public interface IFluentDelete {
		IFluentDeleteFilter From(string tableName);
	}

	public interface IFluentDeleteFilter {
		int AllRows();
		int Where(Filter where);
	}
}