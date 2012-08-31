using System;
using NUnit.Framework;
using Sharp.Data.Databases;
using Sharp.Data.Databases.MySql;
using Sharp.Data.Providers;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Mysql {
    [TestFixture]
    public class MySqlDataClientFactoryTests : DataClientFactoryTests {
        
        public override string GetDatabaseType() {
            return DataProviderNames.MySql;
        }

        public override Type GetDataProviderType() {
            return typeof (MySqlProvider);
        }

        public override Type GetDataClientType() {
            return typeof(MySqlDataClient);            
        }

        public override Type GetDialectType() {
            return typeof(MySqlDialect);            
        }
    }
}