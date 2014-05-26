using System;
using System.Linq;
using Sharp.Data.Databases;
using Sharp.Migrations.Attributes;

namespace Sharp.Migrations.Runners {
    public class MigrationInfo {
        public MigrationInfo(Type type) {
            MigrationType = type;
            Version = VersionHelper.GetVersion(type);
        }
        public long Version { get; set; }
        public Type MigrationType { get; set; }

        public string Name {
            get {
                return MigrationType.Name;
            }
        }
        public bool MigratesFor(DatabaseKind databaseKind) {
            Attribute[] attrs = Attribute.GetCustomAttributes(MigrationType);
            if (attrs.Length == 0) return true;
            var onlyWhen = (OnlyForAttribute)attrs[0];
            return onlyWhen.DatabaseKinds.Contains(databaseKind);
        }
    }
}