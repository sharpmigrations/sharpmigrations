using System;
using System.Text;

namespace Sharp.Util {
    
    public static class StringHelper {

        public static string Implode(String[] value, string separator) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < value.Length; i++) {
                sb.Append(value[i]);
                if (i != value.Length - 1) {
                    sb.Append(separator);
                }
            }
            return sb.ToString();
        }
    
    }
}
