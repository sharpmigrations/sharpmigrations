using System.Data;

namespace Sharp.Data {
    public class Database : DefaultDatabase, IDatabase {
        public Database(IDataProvider provider, string connectionString)
            : base(provider, connectionString) {
            Timeout = -1;
        }

        public int ExecuteSql(string call, params object[] parameters) {
            return ExecuteCatchingErrors(() => TryExecuteSql(call, parameters), call);
        }

        public int ExecuteSqlCommitAndDispose(string call, params object[] parameters) {
            try {
                return ExecuteSql(call, parameters);
            }
            finally {
                Commit();
                Dispose();
            }
        }

        public int ExecuteBulkSql(string call, params object[] parameters) {
            return ExecuteCatchingErrors(() => TryExecuteSql(call, parameters, isBulk: true), call);
        }

        public int ExecuteBulkSqlCommitAndDispose(string call, params object[] parameters) {
            try {
                return ExecuteBulkSql(call, parameters);
            }
            finally {
                Commit();
                Dispose();
            }
        }

        public ResultSet Query(string call, params object[] parameters) {
            return ExecuteCatchingErrors(() => TryCreateReader(call, parameters, CommandType.Text), call);
        }

        public ResultSet QueryAndDispose(string call, params object[] parameters) {
            try {
                return Query(call, parameters);
            }
            finally {
                Dispose();
            }
        }

        public object QueryScalar(string call, params object[] parameters) {
            return ExecuteCatchingErrors(() => TryQueryReader(call, parameters), call);
        }

        public object QueryScalarAndDispose(string call, params object[] parameters) {
            try {
                return QueryScalar(call, parameters);
            }
            finally {
                Dispose();
            }
        }

        public void ExecuteStoredProcedure(string call, params object[] parameters) {
            ExecuteCatchingErrors(() => TryExecuteStoredProcedure(call, parameters), call);
        }

        public void ExecuteStoredProcedureAndDispose(string call, params object[] parameters) {
            try {
                ExecuteStoredProcedure(call, parameters);
            }
            finally {
                Commit();
                Dispose();
            }
        }

        public void ExecuteBulkStoredProcedure(string call, params object[] parameters) {
            ExecuteCatchingErrors(() => TryExecuteStoredProcedure(call, parameters, isBulk: true), call);
        }

        public void ExecuteBulkStoredProcedureAndDispose(string call, params object[] parameters) {
            try {
                ExecuteBulkStoredProcedure(call, parameters);
            }
            finally {
                Commit();
                Dispose();
            }
        }

        public ResultSet CallStoredProcedure(string call, params object[] parameters) {
            return ExecuteCatchingErrors(() => TryCreateReader(call, parameters, CommandType.StoredProcedure), call);
        }

        public object CallStoredFunction(DbType returnType, string call, params object[] parameters) {
            return ExecuteCatchingErrors(() => TryCallStoredFunction(returnType, call, parameters), call);
        }

        public void Commit() {
            if (Connection == null) {
                return;
            }

            try {
                CommitTransaction();
            }
            finally {
                Close();
            }
        }

        public void RollBack() {
            if (Connection == null) {
                return;
            }

            try {
                RollBackTransaction();
            }
            finally {
                Close();
            }
        }

        public void Dispose() {
            Close();
        }

        public void Close() {
            CloseTransaction();
            CloseConnection();
        }
    }
}