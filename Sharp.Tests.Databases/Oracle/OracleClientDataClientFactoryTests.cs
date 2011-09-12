using System;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Data.Providers;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Oracle {
    public class OracleClientDataClientFactoryTests : DataClientFactoryTests {
        
        public override string GetDatabaseType() {
            return DataProviderNames.OracleClient;
        }

        public override Type GetDataProviderType() {
            return typeof (OracleClientProvider);
        }

        public override Type GetDataClientType() {
            return typeof(DataClient);            
        }

        public override Type GetDialectType() {
            return typeof(OracleDialect);     
        }
    }
}