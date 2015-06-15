using System;
using System.Data;
using System.Data.Common;
using Sharp.Data.Exceptions;

namespace Sharp.Data.Databases.PostgreSql {
    public class PostgreSqlProvider : DataProvider {
        private const string SavepointId = "PostgreSqlId";

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

        public override string CommandToBeExecutedBeforeEachOther() {
            return String.Format("SAVEPOINT {0}", SavepointId);
        }

        public override string CommandToBeExecutedAfterAnExceptionIsRaised() {
            return String.Format("ROLLBACK TO {0}", SavepointId);
        }
    }
}
