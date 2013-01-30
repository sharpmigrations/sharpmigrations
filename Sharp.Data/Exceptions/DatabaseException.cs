using System;
using System.Reflection;
using Sharp.Data.Log;

namespace Sharp.Data {
    public class DatabaseException : Exception {

        private static readonly ILogger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        public string SQL { get; set; }

        public DatabaseException(string message, Exception innerException, string sql)
            : base(message, innerException) {
            SQL = sql;
            Log.Error(ToString());
        }

        public override string ToString() {
            return String.Format("Error running SQL: {0}\r\n{1}", SQL, base.ToString());
        }
    }
}