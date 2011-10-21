using System.Data;

namespace Sharp.Data {
    public interface IDataProvider {
        IDbConnection GetConnection();
        IDbDataParameter GetParameter();
        IDbDataParameter GetParameterCursor();
    }
}