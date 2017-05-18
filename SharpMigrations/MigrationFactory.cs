using System;
using SharpData;

namespace SharpMigrations {
	public class MigrationFactory {
		private readonly IDataClient _dataClient;

		public MigrationFactory(IDataClient dataClient) {
			_dataClient = dataClient;
		}

		public Migration CreateMigration(Type migrationType) {
			var migration = (Migration) Activator.CreateInstance(migrationType);
			migration.SetDataClient(_dataClient);
			return migration;
		}
	}
}