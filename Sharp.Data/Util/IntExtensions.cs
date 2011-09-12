using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharp.Data.Util {
    public static class IntExtensions {

        public static bool Between(this Int32 value, int smaller, int bigger) {
            return value >= smaller && value <= bigger;
        }
    }
}
