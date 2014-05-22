using System;
using System.Data;
using System.Text;
using Sharp.Data.Log;

namespace Sharp.Data {
	public class Database : IDatabase {
        private static readonly ISharpLogger Log = LogManager.GetLogger("Sharp.Data.Database");

		protected IDbConnection _connection;
		protected IDbTransaction _transaction;

		public IDataProvider Provider { get; protected set; }
		public string ConnectionString { get; protected set; }
		public int Timeout { get; set; }

		public Database(IDataProvider provider, string connectionString) {
			Timeout = -1;
			Provider = provider;
			ConnectionString = connectionString;
			LogDatabaseProviderName(provider.ToString());
		}

		private static void LogDatabaseProviderName(string providerName) {
			Log.Debug("Provider: " + providerName);
		}
		
        public int ExecuteSql(string call, params object[] parameters) {
			try {
				return TryExecuteSql(call, parameters);
			} catch (Exception ex) {
                throw Provider.CreateSpecificException(ex, call);
			}
		}

	    public int ExecuteBulkSql(string call, params object[] parameters) {
            try {
                return TryExecuteSql(call, parameters, isBulk:true);
            }
            catch (Exception ex) {
                throw Provider.CreateSpecificException(ex, call);
            }
	    }

	    public int ExecuteSqlCommitAndDispose(string call, params object[] parameters) {
			try {
				return ExecuteSql(call, parameters);
			} finally {
				Commit();
				Dispose();
			}
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

		private int TryExecuteSql(string call, object[] parameters, bool isBulk = false) {
            IDbCommand cmd = CreateCommand(call, parameters, isBulk);
			int modifiedRows = cmd.ExecuteNonQuery();
			RetrieveOutParameters(parameters, cmd);
			return modifiedRows;
		}

		private void RetrieveOutParameters(object[] parameters, IDbCommand cmd) {
			if (parameters == null) {
				return;
			}
			foreach (object parameter in parameters) {
				Out pout = parameter as Out;
				if (pout != null) {
					pout.Value = ((IDbDataParameter)cmd.Parameters[pout.Name]).Value;
					continue;
				}
				InOut pinout = parameter as InOut;
				if (pinout != null) {
					pinout.Value = ((IDbDataParameter)cmd.Parameters[pinout.Name]).Value;
					continue;
				}
			}
		}

	    public ResultSet Query(string call, params object[] parameters) {
			IDataReader reader = null;
			try {
				reader = TryCreateReader(call, parameters, CommandType.Text);
				return DataReaderToResultSetMapper.Map(reader);
			} catch (Exception ex) {
				throw Provider.CreateSpecificException(ex, call);
			} finally {
				if (reader != null) {
					reader.Dispose();
				}
			}
		}

		public ResultSet QueryAndDispose(string call, params object[] parameters) {
			try {
				return Query(call, parameters);
			} finally {
				Dispose();
			}
		}

		private IDataReader TryCreateReader(string call, object[] parameters, CommandType commandType) {
			IDbCommand cmd = CreateCommand(call, parameters);
			cmd.CommandType = commandType;
			return cmd.ExecuteReader();
		}

		public object QueryScalar(string call, params object[] parameters) {
			try {
				return TryQueryReader(call, parameters);
			} catch (Exception ex) {
                throw Provider.CreateSpecificException(ex, call);
			}
		}

		public object QueryScalarAndDispose(string call, params object[] parameters) {
			try {
				return QueryScalar(call, parameters);
			} finally {
				Dispose();
			}
		}

		private object TryQueryReader(string call, object[] parameters) {
			IDbCommand cmd = CreateCommand(call, parameters);
			object obj = cmd.ExecuteScalar();
			return obj;
		}

		public void ExecuteStoredProcedure(string call, params object[] parameters) {
			try {
				TryExecuteStoredProcedure(call, parameters);
			} catch (Exception ex) {
                throw Provider.CreateSpecificException(ex, call);
			}
		}

	    public void ExecuteBulkStoredProcedure(string call, params object[] parameters) {
            try {
                TryExecuteStoredProcedure(call, parameters, isBulk:true);
            }
            catch (Exception ex) {
                throw Provider.CreateSpecificException(ex, call);
            }
	    }

	    public void ExecuteStoredProcedureAndDispose(string call, params object[] parameters) {
			try {
				ExecuteStoredProcedure(call, parameters);
			} finally {
				Commit();
				Dispose();
			}
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

		private void TryExecuteStoredProcedure(string call, object[] parameters, bool isBulk = false) {
            IDbCommand cmd = CreateCommand(call, parameters, isBulk);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.ExecuteNonQuery();
			RetrieveOutParameters(parameters, cmd);
		}

		public ResultSet CallStoredProcedure(string call, params object[] parameters) {
			IDataReader reader = null;
			try {
				reader = TryCreateReader(call, parameters, CommandType.StoredProcedure);
				ResultSet res = DataReaderToResultSetMapper.Map(reader);
				return res;
			} catch (Exception ex) {
                throw Provider.CreateSpecificException(ex, call);
			} finally {
				if (reader != null) {
					reader.Dispose();
				}
			}
		}

		public object CallStoredFunction(DbType returnType, string call, params object[] parameters) {
			try {
				return TryCallStoredFunction(returnType, call, parameters);
			} catch (Exception ex) {
                throw Provider.CreateSpecificException(ex, call);
			}
		}

		private object TryCallStoredFunction(DbType returnType, string call, object[] parameters) {
			IDbDataParameter returnPar = GetReturnParameter(returnType);

			IDbCommand cmd = CreateCommand(call, parameters);
			cmd.Parameters.Insert(0, returnPar);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.ExecuteNonQuery();

			object returnObject = returnPar.Value;
			return returnObject;
		}

		private IDbCommand CreateCommand(string call, object[] parameters, bool isBulk = false) {
			OpenConnection();
			IDbCommand cmd = CreateIDbCommand(call, parameters);
		    Provider.ConfigCommand(cmd, parameters, isBulk);
			SetTimeoutForCommand(cmd);
			PopulateCommandParameters(cmd, parameters);
			LogCommandCall(call, cmd);
			return cmd;
		}

		private void SetTimeoutForCommand(IDbCommand cmd) {
			//Doesn't work for oracle!
			if (Timeout >= 0) {
				cmd.CommandTimeout = Timeout;
			}
		}

		private IDbCommand CreateIDbCommand(string call, object[] parameters) {
			IDbCommand cmd = _connection.CreateCommand();
			cmd.CommandText = call;
			cmd.Transaction = _transaction;
			return cmd;
		}

		private static void LogCommandCall(string call, IDbCommand cmd) {
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

		private void PopulateCommandParameters(IDbCommand cmd, object[] parameters) {
			if (parameters == null) {
				return;
			}
			foreach (object parameter in parameters) {
				IDbDataParameter par;
				if (parameter is Out) {
					par = GetOutParameter((Out)parameter);
				} else if (parameter is InOut) {
					par = GetInOutParameter((InOut)parameter);
				} else if (parameter is In) {
					par = GetInParameter((In)parameter, cmd);
				} else {
                    par = GetInParameter(new In { Value = parameter }, cmd);
				}
				//this is for when you have the cursor parameter, ignored by sql server
				if (par != null) {
					cmd.Parameters.Add(par);
				}
			}
		}

        private IDbDataParameter GetInParameter(In p, IDbCommand cmd) {
            IDbDataParameter par = Provider.GetParameter(p);
            par.Direction = ParameterDirection.Input;
            par.Value = p.Value ?? DBNull.Value;
            par.ParameterName = p.Name;
			return par;
		}

		private IDbDataParameter GetOutParameter(Out outParameter) {
			IDbDataParameter par;
			if (outParameter.IsCursor) {
				par = Provider.GetParameterCursor();
			} else {
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

		private IDbDataParameter GetInOutParameter(InOut p) {
			IDbDataParameter par = Provider.GetParameter();

			par.Direction = ParameterDirection.InputOutput;
			par.ParameterName = p.Name;
			par.Size = p.Size;
			par.Value = p.Value ?? DBNull.Value;
			par.DbType = p.Type;
			return par;
		}

		private IDbDataParameter GetReturnParameter(DbType type) {
			IDbDataParameter par = Provider.GetParameter(null);
			par.Direction = ParameterDirection.ReturnValue;
		    par.Size = 4000;
			par.DbType = type;
			return par;
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

		public void Close() {
			CloseTransaction();
			CloseConnection();
		}

		private void CloseTransaction() {
			if (_transaction == null) {
				return;
			}
			try {
				_transaction.Dispose();
			} catch { }
			_transaction = null;
		}

		private void CloseConnection() {
			if (_connection == null) {
				return;
			}
			try {
				_connection.Close();
				_connection.Dispose();
				Log.Debug("Connection closed");
			} catch { }
			_connection = null;
		}

		public void Commit() {
			if (_connection == null) {
				return;
			}

			try {
				CommitTransaction();
			} finally {
				Close();
			}
		}

		private void CommitTransaction() {
			if (_transaction == null) {
				return;
			}
			_transaction.Commit();
			Log.Debug("Commit");
		}

		public void RollBack() {
			if (_connection == null) {
				return;
			}

			try {
				RollBackTransaction();
			} finally {
				Close();
			}
		}

		private void RollBackTransaction() {
			if (_transaction == null) {
				return;
			}
			_transaction.Rollback();
			Log.Debug("Rollback");
		}

		public void Dispose() {
			Close();
		}
	}
}