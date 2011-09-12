using System.Data.Common;
using Sharp.Data.Databases;

namespace Sharp.Data.Providers {
    public class SqlProvider : DataProvider {
        public SqlProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }
    }
}
