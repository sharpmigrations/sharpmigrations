using log4net.Config;
using NUnit.Framework;

namespace Sharp.Tests.Databases.Data {
    [SetUpFixture]
    public class SetupFixture {
        [SetUp]
        public void SetUp() {
            XmlConfigurator.Configure();
        }
    }
}