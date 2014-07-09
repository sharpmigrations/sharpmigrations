using System;
using System.Data;
using Sharp.Data.Fluent;
using Sharp.Data.Schema;
using SColumn = Sharp.Data.Schema.Column;

namespace Sharp.Migrations {
    public abstract class SchemaMigration : Migration {

        private FluentAdd _schemaMigrationAdd;
        private FluentRemove _schemaMigrationRemove;
        private FluentRename _schemaMigrationRename;
        private FluentModify _schemaMigrationModify;

        public IFluentAdd Add {
            get { return _schemaMigrationAdd ?? (_schemaMigrationAdd = new FluentAdd(DataClient)); }
        }

        public FluentRemove Remove {
            get { return _schemaMigrationRemove ?? (_schemaMigrationRemove = new FluentRemove(DataClient)); }
        }

        public FluentRename Rename {
            get { return _schemaMigrationRename ?? (_schemaMigrationRename = new FluentRename(DataClient)); }
        }

        [Obsolete("Was mispelled. Correct name is Modify")]
        public FluentModify Mofify {
            get { return Modify; }
        }

        public FluentModify Modify {
            get { return _schemaMigrationModify ?? (_schemaMigrationModify = new FluentModify(DataClient)); }
        }

        protected class Column {
            public static FluentColumn AutoIncrement(string name) {
                var fc = SColumn.Int32(name);
                fc.Object.IsAutoIncrement = true;
                return fc;
            }
            public static FluentColumn String(string name) { return SColumn.String(name); }
            public static FluentColumn String(string name, int size) { return SColumn.String(name, size); }
            public static FluentColumn Clob(string name) { return SColumn.Clob(name); }
            public static FluentColumn Int16(string name) { return SColumn.Int16(name); }
            public static FluentColumn Int32(string name) { return SColumn.Int32(name); }
            public static FluentColumn Int64(string name) { return SColumn.Int64(name); }
            public static FluentColumn Boolean(string name) {
                return SColumn.Boolean(name);
            }
            public static FluentColumn Binary(string name) { return SColumn.Binary(name); }
            public static FluentColumn Date(string name) { return SColumn.Date(name); }
            public static FluentColumn Decimal(string name) { return SColumn.Decimal(name); }
            public static FluentColumn Single(string name) { return SColumn.Single(name); }
            public static FluentColumn Double(string name) { return SColumn.Double(name); }
            public static FluentColumn Guid(string name) { return SColumn.Guid(name); }
        }
    }
}