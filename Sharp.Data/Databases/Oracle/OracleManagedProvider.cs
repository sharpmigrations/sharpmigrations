using System.Data.Common;

namespace Sharp.Data.Databases.Oracle {
    public class OracleManagedProvider : OracleOdpProvider {
        public override string Name {
            get { return DataProviderNames.OracleManaged; }
        }

        public OracleManagedProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {}


        protected override string RefCursorFullName {
            get { return "Oracle.ManagedDataAccess.Client.OracleDbType"; }
        }
    }
}