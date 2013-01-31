using System;

namespace Sharp.Data.Exceptions {
    public class UniqueConstraintException : DatabaseException {
        public UniqueConstraintException(string message, Exception innerException, string sql)
            : base(message, innerException, sql) {
        }
    }
}