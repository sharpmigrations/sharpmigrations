using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Oracle {
    [TestFixture]
    public class OracleSpecificExceptionsTests : SpecificExceptionsTests {
        [SetUp]
        public void SetUp() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.OracleOdp);
            _database = _dataClient.Database;
        } 
    }
}
