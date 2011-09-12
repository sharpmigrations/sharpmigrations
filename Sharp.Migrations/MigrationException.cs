using System;

namespace Sharp.Migrations {
	public class MigrationException : Exception {
		public MigrationException(string message, Exception innerException) : base(message, innerException) {}
	}
}