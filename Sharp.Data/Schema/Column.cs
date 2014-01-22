using System.Data;

namespace Sharp.Data.Schema {
    public class Column {

        public DbType Type { get; set; }
        public string ColumnName { get; set; }
        public int Size { get; set; }
        public bool IsNullable { get; set; }
        public object DefaultValue { get; set; }
        public string Comment { get; set; }
        
        private bool _isPrimaryKey;
        public bool IsPrimaryKey {
            get {
                return _isPrimaryKey;
            }
            set {
                _isPrimaryKey = value;
                if (_isPrimaryKey) IsNullable = false;
            }
        }

        private bool _isAutoIncrement;
        public bool IsAutoIncrement {
            get {
                return _isAutoIncrement;
            }
            set {
                _isAutoIncrement = value;
                if (_isAutoIncrement) IsNullable = false;
            }
        }

        public Column(string name) : this(name, DbType.String) {}

        public Column(string name, DbType type) {
            ColumnName = name;
            IsAutoIncrement = false;
            Type = type;
            IsNullable = true;
        }
        
        public static FluentColumn AutoIncrement(string name) { 
            var fc = new FluentColumn(name, DbType.Int32);
            fc.Object.IsAutoIncrement = true;
            return fc; 
        }
        public static FluentColumn String(string name) { return new FluentColumn(name, DbType.String); }
        public static FluentColumn String(string name, int size) { return new FluentColumn(name, DbType.String, size); }
        public static FluentColumn Clob(string name) { return new FluentColumn(name, DbType.String, System.Int32.MaxValue); }
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
