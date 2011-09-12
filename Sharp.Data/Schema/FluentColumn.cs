using System.Data;

namespace Sharp.Data.Schema {
    public class FluentColumn {

        public Column Object { get; protected set; }

        public FluentColumn(string name) : this(name, DbType.String, -1) { }

        public FluentColumn(string name, DbType type) : this(name, type, -1) { }

        public FluentColumn(string name, DbType type, int size) {
            Object = new Column(name, type);
            if (size > 0) {
                Object.Size = size;
            }
        }

        public FluentColumn Size(int size) {
            Object.Size = size;
            return this;
        }

        public FluentColumn NotNull() {
            Object.IsNullable = false;
            return this;
        }

        public FluentColumn AsPrimaryKey() {
            Object.IsPrimaryKey = true;
            return this;
        }

        public FluentColumn DefaultValue(object value) {
            Object.DefaultValue = value;
            return this;
        }
    }

    public class FluentColumnTypes {

        private readonly FluentColumn _fluentColumn;

        public FluentColumnTypes(FluentColumn fluentColumn) {
            _fluentColumn = fluentColumn;
        }

        public FluentColumn AutoIncrement(string name) {
            _fluentColumn.Object.Type = DbType.Int32;
            _fluentColumn.Object.IsAutoIncrement = true;
            return _fluentColumn;
        }
        public FluentColumn String { get { _fluentColumn.Object.Type = DbType.String; return _fluentColumn; } }
        public FluentColumn Int16 { get { _fluentColumn.Object.Type = DbType.Int16; return _fluentColumn; } }
        public FluentColumn Int32 { get { _fluentColumn.Object.Type = DbType.Int32; return _fluentColumn; } }
        public FluentColumn Int64 { get { _fluentColumn.Object.Type = DbType.Int64; return _fluentColumn; } }
        public FluentColumn Boolean { get { _fluentColumn.Object.Type = DbType.Boolean; return _fluentColumn; } }
        public FluentColumn Binary { get { _fluentColumn.Object.Type = DbType.Binary; return _fluentColumn; } }
        public FluentColumn Date { get { _fluentColumn.Object.Type = DbType.Date; return _fluentColumn; } }
        public FluentColumn Decimal { get { _fluentColumn.Object.Type = DbType.Decimal; return _fluentColumn; } }
        public FluentColumn Single { get { _fluentColumn.Object.Type = DbType.Single; return _fluentColumn; } }
        public FluentColumn Double { get { _fluentColumn.Object.Type = DbType.Double; return _fluentColumn; } }
        public FluentColumn Guid { get { _fluentColumn.Object.Type = DbType.Guid; return _fluentColumn; } }
    }
}
