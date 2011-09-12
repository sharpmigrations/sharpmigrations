using System.Data;
using System.Data.Common;

namespace Sharp.Data.Databases {
    public abstract class DataProvider : IDataProvider {
        protected DbProviderFactory DbProviderFactory { get; private set; }

        protected DataProvider(DbProviderFactory dbProviderFactory) {
            DbProviderFactory = dbProviderFactory;
        }

        public virtual IDbConnection GetConnection() {
            return DbProviderFactory.CreateConnection();
        }

        public virtual IDbDataParameter GetParameter() {
            return DbProviderFactory.CreateParameter();
        }

        public virtual IDbDataParameter GetParameterCursor() {
            return DbProviderFactory.CreateParameter();
        }
    }
}