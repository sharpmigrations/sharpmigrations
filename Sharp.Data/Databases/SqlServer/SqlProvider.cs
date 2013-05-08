using System.Data.Common;
using System.Data.SqlClient;
using Sharp.Data.Databases;
using Sharp.Data.Exceptions;

namespace Sharp.Data.Providers {
    public class SqlProvider : DataProvider {
        public SqlProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }

        public override DatabaseKind DatabaseKind {
            get { return DatabaseKind.SqlServer; }
        }

        public override DatabaseException CreateSpecificException(System.Exception exception, string sql) {
            var sqlexception = exception as SqlException;
            if (sqlexception == null) return base.CreateSpecificException(exception, sql);
                
            if(sqlexception.Number == 208) {
                return new TableNotFoundException(exception.Message, exception, sql);
            }
            if (sqlexception.Number == 2627) {
                return new UniqueConstraintException(exception.Message, exception, sql);
            }
            return base.CreateSpecificException(exception, sql);
        }
    }
}
