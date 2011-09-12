using System.Collections.Generic;

namespace Sharp.Data.Schema {
    
    public class Table {
        
        public string Name { get; protected set; }
        public List<Column> Columns { get; protected set; }

        public Table(string name) {
            Name = name;
            Columns = new List<Column>();
        }
    }
}