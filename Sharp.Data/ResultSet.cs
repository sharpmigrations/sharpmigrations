using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Sharp.Data {
	public class ResultSet : List<TableRow> {
        private List<string> _originalColumnNames;
		private Dictionary<string, int> _cols;

        public ResultSet(params string[] cols) {
            _cols = new Dictionary<string, int>();
            _originalColumnNames = new List<string>();

            for (int i = 0; i < cols.Length; i++) {
                _originalColumnNames.Add(cols[i]);
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
            return _cols[colName.ToUpper()];
        }

        public string[] GetColumnNames() {
            return _originalColumnNames.ToArray();
        }
    }
}