using System;
using System.Data;

namespace Sharp.Data {
    public interface IDataProvider {
        IDbConnection GetConnection();
        void ConfigCommand(IDbCommand command);
        IDbDataParameter GetParameter();
        IDbDataParameter GetParameterCursor();
        DatabaseException ThrowSpecificException(Exception exception, string sql);
    }
}