using System;
using NUnit.Framework;
using Sharp.Data;

namespace Sharp.Tests.Databases.Data {
    [Explicit]
    public abstract class DataClientFactoryTests {

        string _connectionString = "connectionString";

        [SetUp]
        public void SetUp() {
        }

        [Test]
        public virtual void Can_create_dataclient() {
            IDataClient client = SharpFactory.Default.CreateDataClient(_connectionString, GetDatabaseType());

            //check connection string
            Assert.AreEqual(_connectionString, client.Database.ConnectionString);
            
            //check DataClient type
            Assert.That(client.GetType() == GetDataClientType());

            //check Dialect type
            Assert.That(client.Dialect.GetType() == GetDialectType());

			////check Data provider type
			//Assert.That(client.Database.Provider.GetType() == GetDataProviderType());            
        }

        public abstract string GetDatabaseType();
        public abstract Type GetDataProviderType();
        public abstract Type GetDataClientType();
        public abstract Type GetDialectType();
    }
}