using System.Data.SQLite;
using System.IO;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.SQLite {
    [TestFixture]
	public class SqLiteSchemaTests : DataClientSchemaTests {
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
        public override void Can_add_foreign_key_to_table() {
            try {
                base.Can_add_foreign_key_to_table();
                Assert.Fail();
            }
            catch (NotSupportedByDialect ex) {
                Assert.AreEqual(ex.DialectName, "SqLiteDialect");
                Assert.AreEqual(ex.FunctionName, "GetForeignKeySql");
            }
        }

    	[Test]
        public override void Can_add_named_primary_key_to_table() {
            try {
                base.Can_add_named_primary_key_to_table();
                Assert.Fail();
            }
            catch (NotSupportedByDialect ex) {
                Assert.AreEqual(ex.DialectName, "SqLiteDialect");
                Assert.AreEqual(ex.FunctionName, "GetPrimaryKeySql");
            }
        }

        [Test]
        public override void Can_add_primary_key_to_table() {
            try {
                base.Can_add_primary_key_to_table();
                Assert.Fail();
            }
            catch (NotSupportedByDialect ex) {
                Assert.AreEqual(ex.DialectName, "SqLiteDialect");
                Assert.AreEqual(ex.FunctionName, "GetPrimaryKeySql");
            }
        }

        [Test]
        public override void Can_remove_column_from_table() {
            try {
                base.Can_remove_column_from_table();
                Assert.Fail();
            }
            catch (NotSupportedByDialect ex) {
                Assert.AreEqual(ex.DialectName, "SqLiteDialect");
                Assert.AreEqual(ex.FunctionName, "GetDropColumnSql");
            }
        }

	

        [Test]
        public override void Can_remove_foreign_key_from_table() {
            try {
                _dataClient.RemoveForeignKey("foo", "bar");
                Assert.Fail();
            }
            catch (NotSupportedByDialect ex) {
                Assert.AreEqual(ex.DialectName, "SqLiteDialect");
                Assert.AreEqual(ex.FunctionName, "GetDropForeignKeySql");
            }
        }
    }
}