using System;
using Sharp.Data.Databases;
using Sharp.Data.Databases.PostgreSql;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.PostgreSql {
    public class PostgreSqlDataClientFactoryTests : DataClientFactoryTests {
        public override string GetDatabaseType() {
            return DataProviderNames.PostgreSql;
        }

        public override Type GetDataProviderType() {
            return typeof(PostgreSqlProvider);
        }

        public override Type GetDataClientType() {
            return typeof(PostgreSqlDataClient);
        }

        public override Type GetDialectType() {
            return typeof(PostgreSqlDialect);
        }
    }
}
