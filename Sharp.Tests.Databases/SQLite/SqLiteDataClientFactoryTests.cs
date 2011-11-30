using System;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Data.Databases.SqLite;
using Sharp.Data.Providers;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.SQLite {
    public class SqLiteDataClientFactoryTests : DataClientFactoryTests {
        public override string GetDatabaseType() {
            return DataProviderNames.SqLite;
        }

        public override Type GetDataProviderType() {
            return typeof (SqLiteProvider);
        }

        public override Type GetDataClientType() {
            return typeof (DataClient);
        }

        public override Type GetDialectType() {
            return typeof (SqLiteDialect);
        }
    }
}