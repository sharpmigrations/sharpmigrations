using System;
using System.Collections.Generic;
using Sharp.Data.Databases;

namespace Sharp.Migrations.Attributes {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class OnlyForAttribute : Attribute {
        public DatabaseKind[] DatabaseKinds { get; set; }

        public OnlyForAttribute(params DatabaseKind[] databaseKinds) {
            DatabaseKinds = databaseKinds;
        }
    }
}
