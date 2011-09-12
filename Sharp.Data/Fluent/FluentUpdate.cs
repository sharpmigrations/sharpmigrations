using Sharp.Data.Filters;

namespace Sharp.Data.Fluent {

	public class FluentUpdate : IFluentUpdate, IFluentUpdateColumns, IFluentUpdateValues, IFluentUpdateFilter {

        private Update _update;
        
        public FluentUpdate(IDataClient client) {
            _update = new Update(client);
        }

        public IFluentUpdateColumns Table(string tableName) {
            _update.TableName = tableName;
            return this;
        }

		public IFluentUpdateValues SetColumns(params string[] columnNames) {
			_update.Columns = columnNames;
			return this;
		}

		public IFluentUpdateFilter ToValues(params object[] values) {
			_update.Values = values;
			return this;
		}

		public int AllRows() {
			_update.Execute();
			return _update.AfectedRows;
		}

		public int Where(Filter where) {
			_update.Filter = where;
			_update.Execute();
			return _update.AfectedRows;
		}
    }

	public interface IFluentUpdate {
		IFluentUpdateColumns Table(string tableName);
	}

	public interface IFluentUpdateColumns {
		IFluentUpdateValues SetColumns(params string[] columnNames);
	}

	public interface IFluentUpdateValues {
		IFluentUpdateFilter ToValues(params object[] values);
	}

	public interface IFluentUpdateFilter {
		int AllRows();
		int Where(Filter where);
	}
}