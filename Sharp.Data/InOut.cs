using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data;

namespace Sharp.Data {
	[DebuggerDisplay("[{Name}][{Value}]")]
    public class InOut {
        public string Name { get; set; }
        public object Value { get; set; }
        public DbType Type { get; set; }
        public int Size { get; set; }

        public static InOut Par(string name, DbType type) { return new InOut() { Name = name, Type = type, Size = 4000 }; }
        public static InOut Par(string name, DbType type, int size) { return new InOut() { Name = name, Type = type, Size = size }; }
    }
}
