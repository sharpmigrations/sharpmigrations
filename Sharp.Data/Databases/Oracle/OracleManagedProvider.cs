using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Sharp.Data.Providers;
using Sharp.Data.Util;

namespace Sharp.Data.Databases.Oracle {
    public class OracleManagedProvider : OracleOdpProvider {
        public OracleManagedProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }

        protected override string RefCursorFullName {
            get {
                return "Oracle.ManagedDataAccess.Client.OracleDbType";
            }
        }
    }
}
