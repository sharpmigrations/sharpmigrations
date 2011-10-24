using Sharp.Data.Filters;
using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {
	public class FluentSelect : IFluentSelect, IFluentSelectTable, IFluentSelectFilter, IFluentSelectOrderBy, IFluentSelectToResultSet {
		private Select _select;

		public FluentSelect(IDataClient client) {
			_select = new Select(client);
		}

		public IFluentSelectTable Columns(params string[] columns) {
			_select.Columns = columns;
			return this;
		}

		public IFluentSelectTable AllColumns() {
			return Columns("*");
		}

        public IFluentSelectFilter From(params string[] tableName) {
			_select.SetTableNames(tableName);
			return this;
		}

		public IFluentSelectOrderBy Where(Filter where) {
			_select.Filter = where;
			return this;
		}

		public IFluentSelectToResultSet OrderBy(params OrderBy[] orderBy) {
			_select.OrderBy = orderBy;
			return this;
		}

		public ResultSet AllRows() {
			_select.Execute();
			return _select.ResultSet;
		}

		public ResultSet SkipTake(int skip, int take) {
			_select.Skip = skip;
			_select.Take = take;
			_select.Execute();
			return _select.ResultSet;
		}
	}

	public interface IFluentSelect {
		IFluentSelectTable Columns(params string[] columns);
		IFluentSelectTable AllColumns();
	}

	public interface IFluentSelectTable {
        IFluentSelectFilter From(params string[] tableNames);
	}

	public interface IFluentSelectFilter {
		IFluentSelectOrderBy Where(Filter where);
		IFluentSelectToResultSet OrderBy(params OrderBy[] orderBys);
		ResultSet SkipTake(int skip, int take);
		ResultSet AllRows();
	}

	public interface IFluentSelectOrderBy {
		IFluentSelectToResultSet OrderBy(params OrderBy[] orderBys);
		ResultSet SkipTake(int skip, int take);
		ResultSet AllRows();
	}

	public interface IFluentSelectToResultSet {
		ResultSet AllRows();
		ResultSet SkipTake(int skip, int take);
	}
}