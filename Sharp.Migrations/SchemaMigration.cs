using System.Data;
using Sharp.Data.Fluent;
using Sharp.Data.Schema;

namespace Sharp.Migrations {
    public abstract class SchemaMigration : Migration {

        private FluentAdd _schemaMigrationAdd;
        private FluentRemove _schemaMigrationRemove;
        internal bool ThrowException {
            set { DataClient.ThrowException = value; }
        }

        public FluentAdd Add {
            get {
                if (_schemaMigrationAdd == null) _schemaMigrationAdd = new FluentAdd(DataClient);
                return _schemaMigrationAdd;
            }
        }

        public FluentRemove Remove {
            get {
                if (_schemaMigrationRemove == null) _schemaMigrationRemove = new FluentRemove(DataClient);
                return _schemaMigrationRemove;
            }
        }

        protected class Column {
            public static FluentColumn AutoIncrement(string name) {
                FluentColumn fc = new FluentColumn(name, DbType.Int32);
                fc.Object.IsAutoIncrement = true;
                return fc;
            }
            public static FluentColumn String(string name) { return new FluentColumn(name, DbType.String); }
            public static FluentColumn String(string name, int size) { return new FluentColumn(name, DbType.String, size); }
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
    }
}