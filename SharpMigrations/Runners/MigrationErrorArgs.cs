using System;

namespace SharpMigrations {
    public class MigrationErrorArgs : EventArgs {
        public string MigrationName {get;}
        public Exception Exception { get; }
        public bool Handled { get; set; }

        public MigrationErrorArgs(string migrationName, Exception ex) {
            MigrationName = migrationName;
            Exception = ex;
        }
    }
}