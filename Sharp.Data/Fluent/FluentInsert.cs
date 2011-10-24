using System;

namespace Sharp.Data.Fluent {

	public class FluentInsert : IFluentInsert, IFluentInsertColumns, IFluentInsertValues, IFluentInsertReturning {

        private Insert _insert;

        public FluentInsert(IDataClient client) {
            _insert = new Insert(client);
        }

		public IFluentInsertColumns Into(string tableName) {
            _insert.SetTableNames(tableName);
			return this;
		}

		public IFluentInsertValues Columns(params string[] columns) {
			_insert.Columns = columns;
			return this;
		}

		public IFluentInsertValues Values(params object[] values) {
			_insert.Values = values;
			_insert.Execute();
			return this;
		}

		public IFluentInsertReturning ValuesAnd(params object[] values) {
			_insert.Values = values;
			return this;
		}

		public T Return<T>(string columnName) {
			_insert.ColumnToReturn = columnName;
			_insert.ColumnToReturnType = typeof(T);
			_insert.Execute();
			return (T)Convert.ChangeType(_insert.ColumnToReturnValue, typeof(T));
		}
    }

	public interface IFluentInsert {
		IFluentInsertColumns Into(string tableName);
	}

	public interface IFluentInsertColumns {
		IFluentInsertValues Columns(params string[] columns);
	}

	public interface IFluentInsertValues {
		IFluentInsertValues Values(params object[] values);
		IFluentInsertReturning ValuesAnd(params object[] values);
	}

	public interface IFluentInsertReturning {
		T Return<T>(string columnName);
	}
}