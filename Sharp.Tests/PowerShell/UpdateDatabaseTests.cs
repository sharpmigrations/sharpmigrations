using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.PowerShell;

namespace Sharp.Tests.PowerShell {
    public class UpdateDatabaseTests {

        [Test]
        public void Test() {
            var ud = new UpdateDatabase();
            ud.Execute();
        }
    }
}
