using System.Reflection;

namespace Sharp.Data.Databases.Oracle {
    public class OracleReflectionCache {
        public bool IsCached { get; set; }
        public PropertyInfo PropParameterDbType { get; set; }
        public PropertyInfo PropBindByName { get; set; }
        public PropertyInfo PropArrayBindCount { get; set; }
        public object DbTypeRefCursor { get; set; }
        public object DbTypeBlob { get; set; }
    }
}