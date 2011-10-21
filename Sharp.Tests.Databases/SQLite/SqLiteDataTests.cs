using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.SQLite {

	[TestFixture]
	public class SqLiteDataTests : DataClientDataTests {

		[SetUp]
		public void SetUp() {
			string fileName = "hot.db3";

			if (File.Exists(fileName)) {
				File.Delete(fileName);
			}

			SQLiteConnection.CreateFile(fileName);

            _dataClient = DBBuilder.GetDataClient(DataProviderNames.SqLite);
		}


		[Test]
		public override void Can_insert_returning_id() {
			try {
				base.Can_insert_returning_id();
				Assert.Fail();
			}
			catch (NotSupportedByDialect ex) {
				Assert.AreEqual(ex.DialectName, "SqLiteDialect");
				Assert.AreEqual(ex.FunctionName, "GetInsertReturningColumnSql");
			}
		}
	}
}