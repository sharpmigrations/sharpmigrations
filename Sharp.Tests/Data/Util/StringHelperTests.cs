using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Sharp.Util;

namespace Sharp.Tests.Data.Util {
    [TestFixture]
    public class StringHelperTests {

        [Test]
        public void ImplodeTest() {
            string[] arr1 = { "foo" };
            string value = StringHelper.Implode(arr1, ",");
            Assert.AreEqual("foo", value);
            
            string[] arr = {"foo","bar"};
            value = StringHelper.Implode(arr, ",");
            Assert.AreEqual("foo,bar", value);

            value = StringHelper.Implode(arr, "");
            Assert.AreEqual("foobar", value);

        }
    }
}
