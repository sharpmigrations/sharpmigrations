using System.Data.Common;
using Sharp.Data.Databases;

namespace Sharp.Data.Providers {
    public class MySqlProvider : DataProvider {
        public MySqlProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }
        public override DatabaseKind DatabaseKind {
            get { return DatabaseKind.MySql; }
        }
    }
}