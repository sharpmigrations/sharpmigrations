using System.Data.Common;
using Sharp.Data.Providers;

namespace Sharp.Data.Databases.Oracle {
    public class OracleManagedProvider : OracleOdpProvider {
        public OracleManagedProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }



        protected override string RefCursorFullName {
            get { return "Oracle.ManagedDataAccess.Client.OracleDbType"; }
        }

        public override string Name {
            get { return DataProviderNames.OracleManaged; }
        }
    }
}