using System;

namespace Sharp.Data.Fluent {
    public abstract class DataClientAction {

        protected string[] _tableNames;
        public bool ThrowException { get; set; }
        public IDataClient DataClient { get; set; }

        protected DataClientAction(IDataClient dataClient) {
            DataClient = dataClient;
            ThrowException = true;
        }

        public void SetTableNames(params string[] tableNames) {
            if(tableNames == null || tableNames.Length == 0) {
                throw new ArgumentException("You have to set a table name");
            }
            _tableNames = tableNames;
        }

        public void Execute() {
            try {
                ExecuteInternal();
            }
            catch (Exception) {
                if (ThrowException) {
                    throw;
                }
            }
        }

        protected abstract void ExecuteInternal();
    }
}