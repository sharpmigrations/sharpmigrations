using System.Data;
using System.Diagnostics;

namespace Sharp.Data {
    [DebuggerDisplay("[{Name}][{Value}]")]
    public class In {
        public string Name { get; set; }
        public object Value { get; set; }
        public DbType? DbType { get; set; }

        public static In Named(string name, object parameter, DbType? dbType = null) {
            return new In { Value = parameter, Name = name, DbType = dbType };
        }

        public static In Par(object parameter, DbType? dbType = null) {
            return new In { Value = parameter, DbType = dbType };
        }
    }
}
