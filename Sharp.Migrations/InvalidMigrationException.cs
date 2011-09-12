using System;

namespace Sharp.Migrations {
    public class InvalidMigrationException : Exception {
        public InvalidMigrationException() {
            
        }
        public InvalidMigrationException(string message)
            : base(message) {
            
        }
        public InvalidMigrationException(string message, Exception innerException)
            : base(message, innerException) {
            
        }
        protected InvalidMigrationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) {
            
        }
    }
}
