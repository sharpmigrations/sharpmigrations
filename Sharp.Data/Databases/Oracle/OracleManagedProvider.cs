using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Sharp.Data.Providers;
using Sharp.Data.Util;

namespace Sharp.Data.Databases.Oracle {
    public class OracleManagedProvider : OracleOdpProvider {
        public override string Name {
            get { return DataProviderNames.OracleManaged; }
        }

        public OracleManagedProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }


        protected override string RefCursorFullName {
            get { return "Oracle.ManagedDataAccess.Client.OracleDbType"; }
        }
    }
}