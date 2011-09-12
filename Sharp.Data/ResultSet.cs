using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharp.Data {

    public class ResultSet : List<TableRow> {

        protected Dictionary<string, int> _cols;

        public ResultSet(params string[] cols) {
            _cols = new Dictionary<string, int>();

            for (int i = 0; i < cols.Length; i++) {
                _cols.Add(cols[i].ToUpper(),i);
            }
        }

        public void AddRow(params object[] row) {
            if (row.Length != _cols.Keys.Count) {
                throw new ArgumentException("Wrong number of columns in row");
            }

            Add(new TableRow(this,row));
        }

        public int GetColumnIndex(string colName) {
            return _cols[colName];
        }

        public string[] GetColumnNames() {
            var x = from p in _cols.Keys
                    select (p.ToString());
            return x.ToArray<string>();
        }
    }
}