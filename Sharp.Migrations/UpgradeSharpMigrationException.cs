using System;

namespace Sharp.Migrations {
    public class UpgradeSharpMigrationException : Exception {
        public UpgradeSharpMigrationException(string message, Exception inner)
            : base(message, inner) {
        }
    }
}