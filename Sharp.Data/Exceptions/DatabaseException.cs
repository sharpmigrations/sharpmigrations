using System;

namespace Sharp.Data {
    public class DatabaseException : Exception {

        public string SQL { get; set; }

        public DatabaseException(string message, Exception innerException, string sql)
            : base(message, innerException) {
            SQL = sql;
        }

        public override string ToString() {
            return String.Format("Error running SQL: {0}\r\n{1}", SQL, base.ToString());
        }
    }
}