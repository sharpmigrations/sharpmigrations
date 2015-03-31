using System;
using System.Data;
using System.Data.Common;
using Sharp.Data.Exceptions;

namespace Sharp.Data.Databases.PostgreSql {
    public class PostgreSqlProvider : DataProvider {
        public PostgreSqlProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) { }

        public override string Name {
            get { return DataProviderNames.PostgreSql; }
        }

        public override DatabaseKind DatabaseKind {
            get { return DatabaseKind.PostgreSql; }
        }
        
        public override DatabaseException CreateSpecificException(Exception exception, string sql) {
            if (exception.Message.Contains("42P01")) {
                return new TableNotFoundException(exception.Message, exception, sql);
            }
            if (exception.Message.Contains("23505")) {
                return new UniqueConstraintException(exception.Message, exception, sql);
            }
            return base.CreateSpecificException(exception, sql);
        }

        public override string ExecuteThisParameterlessSqlBeforeAnyOther() {
            return "SAVEPOINT my_savepoint";
        }

        public override string ExecuteThisParameterlessSqlAfterRaiseAnException() {
            return "ROLLBACK TO my_savepoint";
        }
    }
}
