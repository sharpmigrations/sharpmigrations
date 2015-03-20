using System.Data.Common;

namespace Sharp.Data.Databases.PostgreSql {
    public class PostgreSqlProvider : DataProvider {
        public PostgreSqlProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {}

        public override string Name {
            get { return DataProviderNames.PostgreSql; }
        }

        public override DatabaseKind DatabaseKind {
            get { return DatabaseKind.PostgreSql; }
        }
    }
}
