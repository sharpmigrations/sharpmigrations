using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharp.Data.Fluent {
    public class RemovePrimaryKey : RemoveItem, IRemovePrimaryKeyOfTable {
        public RemovePrimaryKey(IDataClient dataClient, string primaryKeyName) : base(dataClient) {
            ItemName = primaryKeyName;
        }

        public void OfTable(string tableName) {
            FirstTableName = tableName;
            Execute();
        }

        protected override void ExecuteInternal() {
            DataClient.RemovePrimaryKey(FirstTableName, ItemName);
        }
    }

    public interface IRemovePrimaryKeyOfTable {
        void OfTable(string tableName);
    }
}
