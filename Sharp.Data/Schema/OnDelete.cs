using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharp.Data.Schema {
    public enum OnDelete {
        NoAction,
        Cascade,
        SetNull
    }
}
