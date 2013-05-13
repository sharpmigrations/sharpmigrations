using System;
using System.Data;
using Sharp.Data.Databases;

namespace Sharp.Data {
    public interface IDataProvider {
        string Name { get; }
        DatabaseKind DatabaseKind { get; }
        IDbConnection GetConnection();
        void ConfigCommand(IDbCommand command);
        IDbDataParameter GetParameter();
        IDbDataParameter GetParameterCursor();
        DatabaseException CreateSpecificException(Exception exception, string sql);
    }
}