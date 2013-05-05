using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data;

namespace Sharp.Tests.Data.Databases {
    [Explicit]
    public abstract class DialectTests {
        protected string TABLE_NAME = "myTable";
        protected Dialect _dialect;
    }
}
