using System;
using Microsoft.Extensions.Logging;
using SharpData;
using SharpData.Fluent;
using SharpData.Log;
using SharpMigrations.Runners;

namespace SharpMigrations {
    public abstract class SeedMigration {

        public static ILogger Logger { get; set; } = SharpMigrationsLogging.CreateLogger<SeedMigration>();

        private IDataClient _dataClient;

        public IDataClient DataClient => _dataClient;

        public void SetDataClient(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        protected void Log(string msg, params object[] args) {
            Logger.LogInformation(String.Format(msg, args));
        }

        protected IFluentDelete Delete => new FluentDelete(DataClient);
        protected IFluentInsert Insert => new FluentInsert(DataClient);
        protected IFluentSelect Select => new FluentSelect(DataClient);
        protected IFluentUpdate Update => new FluentUpdate(DataClient);

        public void ExecuteSql(string call, params object[] parameters) {
            _dataClient.Database.ExecuteSql(call, parameters);
        }

        public abstract void Up(string param = null);
    }
}