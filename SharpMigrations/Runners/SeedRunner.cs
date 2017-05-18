using System;
using System.Reflection;
using SharpData;
using SharpData.Log;

namespace SharpMigrations.Runners {
    public class SeedRunner : ISeedRunner {
        private IDataClient _dataClient;
        private Assembly _targetAssembly;

        public static readonly ISharpLogger Log = LogManager.GetLogger(typeof(SeedRunner).Name);

        public SeedRunner(IDataClient dataClient, Assembly targetAssembly) {
            _dataClient = dataClient;
            _targetAssembly = targetAssembly;
        }

        public void Run(string seedName, string param = null, string migrationGroup = null) {
            var seedType = MigrationFinder.FindSeed(_targetAssembly, seedName);
            Log.Info("Starting seed migration");
            Log.Info(String.Format("Applying Seed -> [{0}]", seedName));

            var migration = (SeedMigration) Activator.CreateInstance(seedType);
            migration.SetDataClient(_dataClient);
            migration.Up(param);
            _dataClient.Commit();
        }
    }
}