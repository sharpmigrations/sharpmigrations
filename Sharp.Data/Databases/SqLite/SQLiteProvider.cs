using System.Data.Common;
using Sharp.Data.Databases;

namespace Sharp.Data.Providers {
    public class SqLiteProvider : DataProvider {
        public SqLiteProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }

        public override string Name {
            get { return DataProviderNames.SqLite; }
        }

        public override DatabaseKind DatabaseKind {
            get { return DatabaseKind.Oracle;}
        }
    }
}