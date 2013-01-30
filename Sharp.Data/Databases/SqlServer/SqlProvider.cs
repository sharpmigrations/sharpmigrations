using System.Data.Common;
using System.Data.SqlClient;
using Sharp.Data.Databases;
using Sharp.Data.Exceptions;

namespace Sharp.Data.Providers {
    public class SqlProvider : DataProvider {
        public SqlProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }

        public override DatabaseException ThrowSpecificException(System.Exception exception, string sql) {
            var sqlexception = exception as SqlException;
            if (sqlexception != null && sqlexception.Number == 208) {
                return new TableNotFoundException(exception.Message, exception, sql);
            }
            return base.ThrowSpecificException(exception, sql);
        }
    }
}
