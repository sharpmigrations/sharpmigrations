using System;
using Sharp.Data.Filters;

namespace Sharp.Data.Fluent {
	public class FluentCount : IFluentCount, IFluentCountFilter {

		private Count _count;

		public FluentCount(IDataClient client) {
			_count = new Count(client);
		}

		public IFluentCountFilter Table(string tableName) {
			_count.TableName = tableName;
			return this;
		}

		public int AllRows() {
			_count.Execute();
			return _count.CountedRows;
		}

		public int Where(Filter where) {
			_count.Filter = where;
			_count.Execute();
			return _count.CountedRows;
		}
	}

	public interface IFluentCount {
		IFluentCountFilter Table(string tableName);
	}

	public interface IFluentCountFilter {
		int AllRows();
		int Where(Filter where);
	}
}
