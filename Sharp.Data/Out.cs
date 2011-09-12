using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sharp.Data {
    public class Out {

        public string Name { get; set; }
        public object Value { get; set; }
        public DbType Type { get; set; }
        public int Size { get; set; }
        public bool IsCursor { get; set; }

        public static Out Cursor { get { return new Out() { IsCursor = true }; } }

        /// <summary>
        /// Create an Output parameter with name and type
        /// </summary>
        /// <param name="name">The parameter's name.</param>
        /// <param name="type">The parameter's type.</param>
        /// <returns>The output parameter</returns>
        public static Out Par(string name, DbType type) { return new Out() { Name = name, Type = type, Size = 4000 }; }
          
        /// <summary>
        /// Create an Output parameter with name, type and size
        /// </summary>
        /// <param name="name">The parameter's name.</param>
        /// <param name="type">The parameter's type.</param>
        /// <param name="type">The parameter's size.</param>
        /// <returns>The output parameter</returns>
        public static Out Par(string name, DbType type, int size) { return new Out() { Name = name, Type = type, Size = size }; }
    }
}
