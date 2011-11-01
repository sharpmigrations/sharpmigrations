using System;
using System.Data;

namespace Sharp.Data {
    public interface IDatabase : IDisposable {
        IDataProvider Provider { get; }
        string ConnectionString { get; }
		int Timeout { get; set; }
        object CallStoredFunction(DbType returnType, string call);
        object CallStoredFunction(DbType returnType, string call, params object[] parameters);
		ResultSet CallStoredProcedure(string call);        
		ResultSet CallStoredProcedure(string call, params object[] parameters);
        int ExecuteSql(string call);
        int ExecuteSql(string call, params object[] parameters);
        int ExecuteSqlAndDispose(string call, params object[] parameters);
        ResultSet Query(string call);
        ResultSet Query(string call, params object[] parameters);
        ResultSet QueryAndDispose(string call);
        ResultSet QueryAndDispose(string call, params object[] parameters);
        object QueryScalar(string call, params object[] parameters);
        object QueryScalarAndDispose(string call, params object[] parameters);
        void Commit();
        void RollBack();
        void Close();
    }
}