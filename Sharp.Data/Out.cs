using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data;

namespace Sharp.Data {
	[DebuggerDisplay("[{Name}][{Value}]")]
    public class Out {

        public string Name { get; set; }
        public object Value { get; set; }
        public DbType Type { get; set; }
        public int Size { get; set; }
        public bool IsCursor { get; set; }

        public static Out Cursor { get { return new Out { IsCursor = true }; } }
        public static Out Par(string name, DbType type) { return new Out { Name = name, Type = type, Size = 4000 }; }
        public static Out Par(string name, DbType type, int size) { return new Out { Name = name, Type = type, Size = size }; }
    }
}
