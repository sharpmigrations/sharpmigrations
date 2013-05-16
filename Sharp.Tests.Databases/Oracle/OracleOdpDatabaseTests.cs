using NUnit.Framework;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Oracle {
    [TestFixture]
    public class OracleOdpDatabaseTests : DatabaseTests {

        [SetUp]
        public void SetUp() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.OracleOdp);
            _database = _dataClient.Database;
            CleanTables();
        } 

    }

    [TestFixture]
    public class OracleManagedDatabaseTests : DatabaseTests {

        [SetUp]
        public void SetUp() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.OracleManaged);
            _database = _dataClient.Database;
            CleanTables();
        }

    }
}
