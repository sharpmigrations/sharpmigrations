using System;
using Sharp.Data;

namespace Sharp.Migrations {
	public class MigrationFactory {
		private readonly IDataClient _dataClient;

		public MigrationFactory(IDataClient dataClient) {
			_dataClient = dataClient;
		}

		public Migration CreateMigration(Type migrationType) {
			Migration migration = (Migration) Activator.CreateInstance(migrationType);
			migration.SetDataClient(_dataClient);
			return migration;
		}
	}
}