using System;
using System.Linq;
using System.Reflection;
using SharpData.Databases;
using SharpMigrations.Attributes;

namespace SharpMigrations.Runners {
    public class MigrationInfo {
        public long Version { get; set; }
        public Type MigrationType { get; set; }
        public string Name => MigrationType.Name;

        public MigrationInfo(Type type) {
            MigrationType = type;
            Version = VersionHelper.GetVersion(type);
        }
        
        public bool MigratesFor(DatabaseKind databaseKind) {
            var onlyWhen = MigrationType.GetTypeInfo().GetCustomAttribute<OnlyForAttribute>();
            return onlyWhen == null || onlyWhen.DatabaseKinds.Contains(databaseKind);
        }
    }
}