using NUnit.Framework;
using Sharp.Data.Databases;

namespace Sharp.Tests.Databases.Oracle {
    [TestFixture]
    public class OracleManagedDataTests : OracleOdpDataTests {
        [SetUp]
        public override void SetUp() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.OracleManaged);
        }
    }
}