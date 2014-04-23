using System;
using System.Data;
using System.Data.Common;

namespace Sharp.Data.Databases {
    public abstract class DataProvider : IDataProvider {
        protected DbProviderFactory DbProviderFactory { get; private set; }
        public abstract string Name { get; }
        public abstract DatabaseKind DatabaseKind { get; }

        protected DataProvider(DbProviderFactory dbProviderFactory) {
            DbProviderFactory = dbProviderFactory;
        }

        public virtual IDbConnection GetConnection() {
            return DbProviderFactory.CreateConnection();
        }

        public virtual void ConfigCommand(IDbCommand command, object[] parameters, bool isBulk) {}

        public IDbDataParameter GetParameter() {
            return DbProviderFactory.CreateParameter();
        }

        public virtual IDbDataParameter GetParameter(In parameter) {
            return GetParameter();
        }

        public virtual IDbDataParameter GetParameterCursor() {
            return DbProviderFactory.CreateParameter();
        }

        public virtual DatabaseException CreateSpecificException(Exception exception, string sql) {
            return new DatabaseException(exception.Message, exception, sql);
        }
    }
}