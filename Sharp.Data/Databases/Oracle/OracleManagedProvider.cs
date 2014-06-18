using System.Data.Common;

namespace Sharp.Data.Databases.Oracle {
    public class OracleManagedProvider : OracleOdpProvider {
        private static OracleReflectionCache _reflectionCache = new OracleReflectionCache();

        public override OracleReflectionCache ReflectionCache {
            get {
                return _reflectionCache;
            }
        }

        public override string Name {
            get { return DataProviderNames.OracleManaged; }
        }

        public OracleManagedProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {}


        protected override string OracleDbTypeEnumName {
            get { return "Oracle.ManagedDataAccess.Client.OracleDbType"; }
        }
    }
}