using NUnit.Framework;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Oracle {
    [TestFixture]
    public class OracleOdpDatabaseTests : OracleManagedDatabaseTests {

        public override string GetDataProviderName() {
            return DataProviderNames.OracleManaged;
        }

    }
}
