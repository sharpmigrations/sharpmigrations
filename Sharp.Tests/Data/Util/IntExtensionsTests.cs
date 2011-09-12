using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharp.Data.Util;
using NUnit.Framework;

namespace Sharp.Tests.Data.Util {
    [TestFixture]
    public class IntExtensionsTests {

        [Test]
        public void BetweenTests() {
            int ten = 10;

            Assert.IsTrue(ten.Between(9, 11));
            Assert.IsTrue(ten.Between(10, 10));
            Assert.IsTrue(ten.Between(-1, 20));
            Assert.IsFalse(ten.Between(11, 9));
            Assert.IsFalse(ten.Between(-1, 0));
            Assert.IsFalse(ten.Between(11, 12));
        }
    }
}
