using System.Data.Common;

namespace Sharp.Data.Databases.Oracle {
    public class OracleManagedProvider : OracleOdpProvider {
        private static OracleReflectionCache _reflectionCache = new OracleReflectionCache();
        public override OracleReflectionCache ReflectionCache => _reflectionCache;
        public override string Name => DataProviderNames.OracleManaged;
        protected override string OracleDbTypeEnumName => "Oracle.ManagedDataAccess.Client.OracleDbType";
        public OracleManagedProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {}

    }
}