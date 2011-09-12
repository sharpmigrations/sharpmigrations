using System.Data;
using System.Data.Common;
using Sharp.Data.Databases;

namespace Sharp.Data.Providers {
    public class OracleClientProvider : DataProvider {
        public OracleClientProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }

        public override IDbDataParameter GetParameterCursor() {
			System.Data.OracleClient.OracleParameter par = new System.Data.OracleClient.OracleParameter();
			par.OracleType = System.Data.OracleClient.OracleType.Cursor;
            return par;
        }
    }
}