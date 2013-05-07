using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Sharp.Data;

namespace Sharp.Migrations {
    public class SqlToFileDatabase : IDatabase {

        public IDataProvider Provider { get; private set; }
        public string ConnectionString { get; private set; }
        public Dialect Dialect { get; set; }
        public int Timeout { get; set; }
        public List<string> Sqls { get; set; }

        public SqlToFileDatabase(Dialect dialect) {
            Sqls = new List<string>();
            Dialect = dialect;
        }

        public object CallStoredFunction(DbType returnType, string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public ResultSet CallStoredProcedure(string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public int ExecuteSql(string call, params object[] parameters) {
            In[] pars = Dialect.ConvertToNamedParameters(parameters);
            foreach (var par in pars) {
                call = call.Replace(par.Name, Dialect.GetColumnValueToSql(par.Value));
            }
            Sqls.Add(call);
            return 1;
        }

        public int ExecuteSqlCommitAndDispose(string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public void ExecuteStoredProcedure(string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public void ExecuteStoredProcedureAndDispose(string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public ResultSet Query(string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public ResultSet QueryAndDispose(string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public object QueryScalar(string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public object QueryScalarAndDispose(string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public void Commit() {
        }

        public void RollBack() {
        }

        public void Close() {
        }

        public void Dispose() {
        }
    }
}
