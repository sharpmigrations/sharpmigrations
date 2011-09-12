using System.Data.Common;
using Sharp.Data.Databases;

namespace Sharp.Data.Providers {
    public class SqLiteProvider : DataProvider {
        public SqLiteProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }
    }
}