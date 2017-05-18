using System;

namespace SharpMigrations {
    public class UpgradeSharpMigrationException : Exception {
        public UpgradeSharpMigrationException(string message, Exception inner)
            : base(message, inner) {
        }
    }
}