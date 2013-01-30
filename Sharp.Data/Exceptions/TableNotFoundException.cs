using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharp.Data.Exceptions {
    public class TableNotFoundException : DatabaseException {
        public TableNotFoundException(string message, Exception innerException, string sql) : base(message, innerException, sql) {
        }
    }
}
