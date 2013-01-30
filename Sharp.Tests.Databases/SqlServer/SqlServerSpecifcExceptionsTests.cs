using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.SqlServer {
    [TestFixture]
    public class SqlServerSpecifcExceptionsTests : SpecificExceptionsTests {
        [SetUp]
        public void SetUp() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.SqlServer);
            _database = _dataClient.Database;
        } 
    }
}
