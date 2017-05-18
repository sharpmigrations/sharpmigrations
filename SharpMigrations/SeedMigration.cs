using System;
using SharpData;
using SharpData.Fluent;
using SharpData.Log;

namespace SharpMigrations {
    public abstract class SeedMigration {

        private static ISharpLogger _log = LogManager.GetLogger(typeof(SeedMigration).Name);

        private IDataClient _dataClient;

        public IDataClient DataClient => _dataClient;

        public void SetDataClient(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        protected void Log(string msg, params object[] args) {
            _log.Info(String.Format(msg, args));
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