using System;
using System.Collections.Generic;
using System.Data;

namespace Sharp.Data {
	public class DataReaderToResultSetMapper {

		public static ResultSet Map(IDataReader dr) {
			
			int numberOfColumns = dr.FieldCount;
			
			string[] colNames = GetColumnNames(dr, numberOfColumns);

			ResultSet table = new ResultSet(colNames);
			
			while (dr.Read()) {
				MapRow(dr, numberOfColumns, table);
			}
			return table;
		}

		private static void MapRow(IDataReader dr, int numberOfColumns, ResultSet table) {
			object[] row = new object[numberOfColumns];

			for (int i = 0; i < numberOfColumns; i++) {
				row[i] = (DBNull.Value.Equals(dr[i])) ? null : dr[i];
			}
			table.AddRow(row);
		}

		private static string[] GetColumnNames(IDataReader dr, int numberOfColumns) {
			List<string> colNames = new List<string>();
			for (int i = 0; i < numberOfColumns; i++) {
				colNames.Add(dr.GetName(i));
			}
			return colNames.ToArray();
		}
	}
}
