using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using SharpData;

namespace SharpMigrations.Runners {
    public class SeedRunner : ISeedRunner {
        private IDataClient _dataClient;
        private Assembly _targetAssembly;

        public static ILogger Logger { get; set; } = SharpMigrationsLogging.CreateLogger<SeedRunner>();

        public SeedRunner(IDataClient dataClient, Assembly targetAssembly) {
            _dataClient = dataClient;
            _targetAssembly = targetAssembly;
        }

        public void Run(string seedName, string param = null) {
            var seedType = MigrationFinder.FindSeed(_targetAssembly, seedName);
            Log("Starting seed migration");
            var msg = String.Format("Applying Seed -> [{0}]", seedName);
            if (param != null) {
                msg += " Param: "+ param;
            }
            Log(msg);

            var migration = (SeedMigration) Activator.CreateInstance(seedType);
            migration.SetDataClient(_dataClient);
            migration.Up(param);
            _dataClient.Commit();
            Log("Seed Migration: success");
        }

        protected virtual void Log(string message) {
            Console.WriteLine(message);
            Logger.LogInformation(message);
        }
    }
}