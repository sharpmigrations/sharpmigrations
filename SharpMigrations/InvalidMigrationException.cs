using System;

namespace SharpMigrations {
    public class InvalidMigrationException : Exception {
        public InvalidMigrationException() {
            
        }
        public InvalidMigrationException(string message)
            : base(message) {
            
        }
        public InvalidMigrationException(string message, Exception innerException)
            : base(message, innerException) {
            
        }
    }
}