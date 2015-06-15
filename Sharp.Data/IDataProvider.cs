using System;
using System.Data;
using Sharp.Data.Databases;

namespace Sharp.Data {
    public interface IDataProvider {
        string Name { get; }
        DatabaseKind DatabaseKind { get; }
        IDbConnection GetConnection();
        void ConfigCommand(IDbCommand command, object[] parameters, bool isBulk);
        IDbDataParameter GetParameter();
        IDbDataParameter GetParameter(In parameter, bool isBulk);
        IDbDataParameter GetParameterCursor();
        DatabaseException CreateSpecificException(Exception exception, string sql);
        string CommandToBeExecutedBeforeEachOther();
        string CommandToBeExecutedAfterAnExceptionIsRaised();
    }
}