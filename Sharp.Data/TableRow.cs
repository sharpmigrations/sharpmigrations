using System.Collections.Generic;

namespace Sharp.Data {
    public class TableRow : List<object> {

        protected ResultSet _table;

        public TableRow(ResultSet table, object[] values) {
            _table = table;
            this.AddRange(values);
        }

        public object this[string col] {
            get {
                return this[_table.GetColumnIndex(col.ToUpper())];
            }
        }

		public string[] GetColumnNames() {
			return _table.GetColumnNames();
		}
    }
}