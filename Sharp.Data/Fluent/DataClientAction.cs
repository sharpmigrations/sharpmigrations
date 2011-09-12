using System;

namespace Sharp.Data.Fluent {
    public abstract class DataClientAction {

        public bool ThrowException { get; set; }
        public IDataClient DataClient { get; set; }

        public string TableName { get; set; }

        public DataClientAction(IDataClient dataClient) {
            DataClient = dataClient;
            ThrowException = true;
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