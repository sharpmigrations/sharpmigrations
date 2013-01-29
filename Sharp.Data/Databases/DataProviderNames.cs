using System.Collections.Generic;

namespace Sharp.Data.Databases {
    public static class DataProviderNames {
        public static string OracleManaged = "Oracle.ManagedDataAccess.Client";
        public static string OracleOdp = "Oracle.DataAccess.Client";
        public static string MySql = "MySql.Data.MySqlClient";
        public static string SqlServer = "System.Data.SqlClient";
        public static string SqLite = "System.Data.SQLite";
        public static string OleDb = "System.Data.OleDb";

        public static List<string> All = new List<string> {
            OracleManaged,
            OracleOdp,
            MySql,
            SqlServer,
            SqLite,
            OleDb
        };
    }
}
