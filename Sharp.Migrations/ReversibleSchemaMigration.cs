using System.Linq;
using System.Collections.Generic;
using System.Data;
using Sharp.Data;
using Sharp.Data.Fluent;
using Sharp.Data.Schema;

namespace Sharp.Migrations {
    public abstract class ReversibleSchemaMigration : Migration {

        private List<DataClientAction> _reversibleActions;
        private FluentAdd _schemaMigrationAdd;
        private FluentRename _schemaMigrationRename;
        
        public IFluentAdd Add {
            get { return _schemaMigrationAdd ?? (_schemaMigrationAdd = new FluentAdd(DataClient)); }
        }

        public IFluentRename Rename {
            get { return _schemaMigrationRename ?? (_schemaMigrationRename = new FluentRename(DataClient)); }
        }

        protected class Column {
            public static FluentColumn AutoIncrement(string name) {
                var fc = new FluentColumn(name, DbType.Int32);
                fc.Object.IsAutoIncrement = true;
                return fc;
            }
            public static FluentColumn String(string name) { return new FluentColumn(name, DbType.String); }
            public static FluentColumn String(string name, int size) { return new FluentColumn(name, DbType.String, size); }

            public static FluentColumn Clob(string name) {
                return Data.Schema.Column.Clob(name);
            }
            public static FluentColumn Int16(string name) { return new FluentColumn(name, DbType.Int16); }
            public static FluentColumn Int32(string name) { return new FluentColumn(name, DbType.Int32); }
            public static FluentColumn Int64(string name) { return new FluentColumn(name, DbType.Int64); }
            public static FluentColumn Boolean(string name) { return new FluentColumn(name, DbType.Boolean); }
            public static FluentColumn Binary(string name) { return new FluentColumn(name, DbType.Binary); }
            public static FluentColumn Date(string name) { return new FluentColumn(name, DbType.Date); }
            public static FluentColumn Decimal(string name) { return new FluentColumn(name, DbType.Decimal); }
            public static FluentColumn Single(string name) { return new FluentColumn(name, DbType.Single); }
            public static FluentColumn Double(string name) { return new FluentColumn(name, DbType.Double); }
            public static FluentColumn Guid(string name) { return new FluentColumn(name, DbType.Guid); }
        }

        public sealed override void Down() {
            _reversibleActions = new List<DataClientAction>();
            _schemaMigrationAdd = new FluentAdd(new FakeDataClient());
            _schemaMigrationRename = new FluentRename(new FakeDataClient());

            _schemaMigrationAdd.OnAction += a => _reversibleActions.Add(a);
            _schemaMigrationRename.OnAction += a => _reversibleActions.Add(a);

            Up();

            IEnumerable<DataClientAction> downActions = _reversibleActions.Select(x => {
                x.DataClient = DataClient;
                return x.ReverseAction();
            }).Reverse();
            foreach (var action in downActions) {
                action.Execute();
            }
        }
    }
}