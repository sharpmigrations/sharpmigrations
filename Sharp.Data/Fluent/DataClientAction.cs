using System;

namespace Sharp.Data.Fluent {
    public abstract class DataClientAction {

        public string[] TableNames { get; protected set; }
        public IDataClient DataClient { get; set; }

        protected DataClientAction(IDataClient dataClient) {
            DataClient = dataClient;
        }

        public void SetTableNames(params string[] tableNames) {
            if(tableNames == null || tableNames.Length == 0) {
                throw new ArgumentException("You have to set a table name");
            }
            TableNames = tableNames;
        }

        public string FirstTableName {
            get { return TableNames[0]; }
            set {SetTableNames(value);}
        }

        public void Execute() {
            ExecuteInternal();
        }

        protected abstract void ExecuteInternal();
        public virtual DataClientAction ReverseAction() {
            throw new NotSupportedException("Can't reverse " + GetType());
        }
    }
}