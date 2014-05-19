using System;
using System.Collections.Generic;
using System.Data;

namespace Sharp.Data {
	public class DataReaderToResultSetMapper {

		public static ResultSet Map(IDataReader dr) {
			int numberOfColumns = dr.FieldCount;
			string[] colNames = GetColumnNames(dr, numberOfColumns);
			var table = new ResultSet(colNames);
			while (dr.Read()) {
				MapRow(dr, numberOfColumns, table);
			}
			return table;
		}

		private static void MapRow(IDataReader dr, int numberOfColumns, ResultSet table) {
			var row = new object[numberOfColumns];
			for (int i = 0; i < numberOfColumns; i++) {
				row[i] = (DBNull.Value.Equals(dr[i])) ? null : dr[i];
			}
			table.AddRow(row);
		}

		private static string[] GetColumnNames(IDataReader dr, int numberOfColumns) {
			var colNames = new List<string>();
			for (int i = 0; i < numberOfColumns; i++) {
				colNames.Add(dr.GetName(i));
			}
			return colNames.ToArray();
		}
	}
}
