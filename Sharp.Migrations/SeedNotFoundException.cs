using System;

namespace Sharp.Migrations {
    public class SeedNotFoundException : MigrationException {
        public SeedNotFoundException(string seedName, string message, Exception innerException = null) : base(message, innerException) { }
    }
}