using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.SqlServer {
	public class SqlServerSchemaTests : DataClientSchemaTests {
		[SetUp]
		public void SetUp() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.SqlServer);
		}
	}
}