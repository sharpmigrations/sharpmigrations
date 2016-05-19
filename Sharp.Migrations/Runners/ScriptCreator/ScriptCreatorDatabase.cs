using System;
using System.Collections.Generic;
using System.Data;
using Sharp.Data;

namespace Sharp.Migrations.Runners.ScriptCreator {
    public class ScriptCreatorDatabase : IDatabase {
        private readonly IDatabase _databaseForReading;

        public IDataProvider Provider { get { return _databaseForReading.Provider; } }
        public string ConnectionString { get; private set; }
        public Dialect Dialect { get; set; }
        public int Timeout { get; set; }
        public List<string> Sqls { get; set; }

        public ScriptCreatorDatabase(Dialect dialect, IDatabase databaseForReading) {
            _databaseForReading = databaseForReading;
            Sqls = new List<string>();
            Dialect = dialect;
        }

        private void AddSql(string sql) {
            Sqls.Add(sql);
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
            AddSql(call);
            return 1;
        }

        public int ExecuteBulkSql(string call, params object[] parameters) {
            return ExecuteSql(call, parameters);
        }

        public int ExecuteSqlCommitAndDispose(string call, params object[] parameters) {
            return ExecuteSql(call, parameters);
        }

        public int ExecuteBulkSqlCommitAndDispose(string call, params object[] parameters) {
            return ExecuteSql(call, parameters);
        }

        public void ExecuteStoredProcedure(string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public void ExecuteBulkStoredProcedure(string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public void ExecuteBulkStoredProcedureAndDispose(string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public void ExecuteStoredProcedureAndDispose(string call, params object[] parameters) {
            throw new NotImplementedException();
        }

        public ResultSet Query(string call, params object[] parameters) {
            return _databaseForReading.Query(call, parameters);
        }

        public ResultSet QueryAndDispose(string call, params object[] parameters) {
            return Query(call, parameters);
        }

        public object QueryScalar(string call, params object[] parameters) {
            return _databaseForReading.QueryScalar(call, parameters);
        }

        public object QueryScalarAndDispose(string call, params object[] parameters) {
            return QueryScalar(call, parameters);
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
