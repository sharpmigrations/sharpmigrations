using System;
using System.Data;

namespace Sharp.Data {
    public interface IDatabase : IDisposable {
        IDataProvider Provider { get; }
        string ConnectionString { get; }
		int Timeout { get; set; }
        object CallStoredFunction(DbType returnType, string call, params object[] parameters);
		ResultSet CallStoredProcedure(string call, params object[] parameters);
        int ExecuteSql(string call, params object[] parameters);
        int ExecuteBulkSql(string call, params object[] parameters);
        int ExecuteSqlCommitAndDispose(string call, params object[] parameters);
        int ExecuteBulkSqlCommitAndDispose(string call, params object[] parameters);
		void ExecuteStoredProcedure(string call, params object[] parameters);
		void ExecuteBulkStoredProcedure(string call, params object[] parameters);
        void ExecuteBulkStoredProcedureAndDispose(string call, params object[] parameters);
        ResultSet Query(string call, params object[] parameters);
        ResultSet QueryAndDispose(string call, params object[] parameters);
        object QueryScalar(string call, params object[] parameters);
        object QueryScalarAndDispose(string call, params object[] parameters);
        void Commit();
        void RollBack();
        void Close();
    }
}