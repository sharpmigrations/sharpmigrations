using System;

namespace SharpMigrations {
    public class SeedNotFoundException : MigrationException {
        public SeedNotFoundException(string seedName, string message, Exception innerException = null) : base(message, innerException) { }
    }
}