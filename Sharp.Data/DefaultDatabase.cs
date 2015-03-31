using System;
using System.Data;
using System.Text;
using Sharp.Data.Log;

namespace Sharp.Data {
    public class DefaultDatabase {
        private static readonly ISharpLogger Log = LogManager.GetLogger("Sharp.Data.Database");

        public IDataProvider Provider { get; protected set; }
        public string ConnectionString { get; protected set; }
        public int Timeout { get; set; }
        
        protected IDbConnection _connection;
        protected IDbTransaction _transaction;

        public DefaultDatabase(IDataProvider provider, string connectionString) {
            Provider = provider;
            ConnectionString = connectionString;
            LogDatabaseProviderName(provider.ToString());
        }

        protected static void LogDatabaseProviderName(string providerName) {
            Log.Debug("Provider: " + providerName);
        }

        protected void RetrieveOutParameters(object[] parameters, IDbCommand cmd) {
            if (parameters == null) {
                return;
            }
            foreach (object parameter in parameters) {
                Out pout = parameter as Out;
                if (pout != null) {
                    pout.Value = ((IDbDataParameter) cmd.Parameters[pout.Name]).Value;
                    continue;
                }
                InOut pinout = parameter as InOut;
                if (pinout != null) {
                    pinout.Value = ((IDbDataParameter) cmd.Parameters[pinout.Name]).Value;
                    continue;
                }
            }
        }

        protected IDataReader TryCreateReader(string call, object[] parameters, CommandType commandType) {
            IDbCommand cmd = CreateCommand(call, parameters);
            cmd.CommandType = commandType;
            return cmd.ExecuteReader();
        }


        protected object TryQueryReader(string call, object[] parameters) {
            IDbCommand cmd = CreateCommand(call, parameters);
            object obj = cmd.ExecuteScalar();
            return obj;
        }

        protected void TryExecuteStoredProcedure(string call, object[] parameters, bool isBulk = false) {
            IDbCommand cmd = CreateCommand(call, parameters, isBulk);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            RetrieveOutParameters(parameters, cmd);
        }

        protected object TryCallStoredFunction(DbType returnType, string call, object[] parameters) {
            IDbDataParameter returnPar = GetReturnParameter(returnType);

            IDbCommand cmd = CreateCommand(call, parameters);
            cmd.Parameters.Insert(0, returnPar);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();

            object returnObject = returnPar.Value;
            return returnObject;
        }
        
        protected void SetTimeoutForCommand(IDbCommand cmd) {
            //Doesn't work for oracle!
            if (Timeout >= 0) {
                cmd.CommandTimeout = Timeout;
            }
        }

        protected IDbCommand CreateIDbCommand(string call, object[] parameters) {
            IDbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = call;
            cmd.Transaction = _transaction;
            return cmd;
        }

        protected static void LogCommandCall(string call, IDbCommand cmd) {
            if (Log.IsDebugEnabled) {
                var sb = new StringBuilder();
                sb.Append("Call: ").AppendLine(call);
                foreach (IDbDataParameter p in cmd.Parameters) {
                    sb.Append(p.Direction).Append("-> ").Append(p.ParameterName);
                    if (p.Value != null) {
                        sb.Append(": ").Append(p.Value);
                    }
                    sb.AppendLine();
                }
                Log.Debug(sb.ToString());
            }
        }

        protected void PopulateCommandParameters(IDbCommand cmd, object[] parameters, bool isBulk) {
            if (parameters == null) {
                return;
            }
            foreach (object parameter in parameters) {
                IDbDataParameter par;
                if (parameter is Out) {
                    par = GetOutParameter((Out) parameter);
                }
                else if (parameter is InOut) {
                    par = GetInOutParameter((InOut) parameter);
                }
                else if (parameter is In) {
                    par = GetInParameter((In) parameter, cmd, isBulk);
                }
                else {
                    par = GetInParameter(new In { Value = parameter }, cmd, isBulk);
                }
                //this is for when you have the cursor parameter, ignored by sql server
                if (par != null) {
                    cmd.Parameters.Add(par);
                }
            }
        }

        protected IDbDataParameter GetInParameter(In p, IDbCommand cmd, bool isBulk) {
            IDbDataParameter par = Provider.GetParameter(p, isBulk);
            par.Direction = ParameterDirection.Input;
            par.Value = p.Value ?? DBNull.Value;
            par.ParameterName = p.Name;
            return par;
        }

        protected IDbDataParameter GetOutParameter(Out outParameter) {
            IDbDataParameter par;
            if (outParameter.IsCursor) {
                par = Provider.GetParameterCursor();
            }
            else {
                par = Provider.GetParameter();
            }
            //this "if != null" is for the cursor parameter, ignored by sql server
            if (par != null) {
                par.Direction = ParameterDirection.Output;
                par.ParameterName = outParameter.Name;
                par.Size = outParameter.Size;
                par.Value = outParameter.Value;
                par.DbType = outParameter.Type;
            }
            return par;
        }

        protected IDbDataParameter GetInOutParameter(InOut p) {
            IDbDataParameter par = Provider.GetParameter();

            par.Direction = ParameterDirection.InputOutput;
            par.ParameterName = p.Name;
            par.Size = p.Size;
            par.Value = p.Value ?? DBNull.Value;
            par.DbType = p.Type;
            return par;
        }

        protected IDbDataParameter GetReturnParameter(DbType type) {
            IDbDataParameter par = Provider.GetParameter(null, false);
            par.Direction = ParameterDirection.ReturnValue;
            par.Size = 4000;
            par.DbType = type;
            return par;
        }

        protected void CloseTransaction() {
            if (_transaction == null) {
                return;
            }
            try {
                _transaction.Dispose();
            }
            catch { }
            _transaction = null;
        }

        protected void CloseConnection() {
            if (_connection == null) {
                return;
            }
            try {
                _connection.Close();
                _connection.Dispose();
                Log.Debug("Connection closed");
            }
            catch { }
            _connection = null;
        }

        protected void CommitTransaction() {
            if (_transaction == null) {
                return;
            }
            _transaction.Commit();
            Log.Debug("Commit");
        }

        protected void RollBackTransaction() {
            if (_transaction == null) {
                return;
            }
            _transaction.Rollback();
            Log.Debug("Rollback");
        }
        protected int TryExecuteSql(string call, object[] parameters, bool isBulk = false) {
            IDbCommand cmd = CreateCommand(call, parameters, isBulk);
            int modifiedRows = cmd.ExecuteNonQuery();
            RetrieveOutParameters(parameters, cmd);
            return modifiedRows;
        }

        protected IDbCommand CreateCommand(string call, object[] parameters, bool isBulk = false) {
            OpenConnection();
            IDbCommand cmd = CreateIDbCommand(call, parameters);
            Provider.ConfigCommand(cmd, parameters, isBulk);
            SetTimeoutForCommand(cmd);
            PopulateCommandParameters(cmd, parameters, isBulk);
            LogCommandCall(call, cmd);
            return cmd;
        }

        protected void OpenConnection() {
            if (_connection != null) {
                return;
            }
            _connection = Provider.GetConnection();
            _connection.ConnectionString = ConnectionString;
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            Log.Debug("Connection open");
        }

        protected ResultSet ExecuteCatchingErrors(Func<IDataReader> getReader, string call) {
            IDataReader reader = null;
            try {
                VerifyIfExistisACommandToBeExecutedBeforeAndExecuteIt();
                reader = getReader();
                return DataReaderToResultSetMapper.Map(reader);
            }
            catch (Exception ex) {
                VerifyIfExistisACommandToBeExecutedOnErrorAndExecuteIt();
                throw Provider.CreateSpecificException(ex, call);
            }
            finally {
                if (reader != null) {
                    reader.Dispose();
                }
            }
        }

        protected T ExecuteCatchingErrors<T>(Func<T> action, string call) {
            try {
                VerifyIfExistisACommandToBeExecutedBeforeAndExecuteIt();
                return action();
            }
            catch (Exception ex) {
                VerifyIfExistisACommandToBeExecutedOnErrorAndExecuteIt();
                throw Provider.CreateSpecificException(ex, call);
            }
        }

        protected void ExecuteCatchingErrors(Action action, string call) {
            try {
                VerifyIfExistisACommandToBeExecutedBeforeAndExecuteIt();
                action();
            }
            catch (Exception ex) {
                VerifyIfExistisACommandToBeExecutedOnErrorAndExecuteIt();
                throw Provider.CreateSpecificException(ex, call);
            }
        }

        protected void VerifyIfExistisACommandToBeExecutedOnErrorAndExecuteIt() {
            if (!String.IsNullOrEmpty(Provider.CommandToBeExecutedAfterAnExceptionIsRaised())) {
                TryExecuteSql(Provider.CommandToBeExecutedAfterAnExceptionIsRaised(), new object[] { });
            }
        }

        protected void VerifyIfExistisACommandToBeExecutedBeforeAndExecuteIt() {
            if (!String.IsNullOrEmpty(Provider.CommandToBeExecutedBeforeEachOther())) {
                TryExecuteSql(Provider.CommandToBeExecutedBeforeEachOther(), new object[] { });
            }
        }
    }
}