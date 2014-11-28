using System;
using System.Reflection;
using Sharp.Data;
using Sharp.Data.Fluent;
using Sharp.Data.Log;

namespace Sharp.Migrations {
    public abstract class SeedMigration {

        private static ISharpLogger _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        private IDataClient _dataClient;

        public IDataClient DataClient {
            get { return _dataClient; }
        }

        public void SetDataClient(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        protected void Log(string msg, params object[] args) {
            _log.Info(String.Format(msg, args));
        }

        protected IFluentDelete Delete { get { return new FluentDelete(DataClient); } }
        protected IFluentInsert Insert { get { return new FluentInsert(DataClient); } }
        protected IFluentSelect Select { get { return new FluentSelect(DataClient); } }
        protected IFluentUpdate Update { get { return new FluentUpdate(DataClient); } }

        public void ExecuteSql(string call, params object[] parameters) {
            _dataClient.Database.ExecuteSql(call, parameters);
        }

        public abstract void Up(string param = null);
    }
}