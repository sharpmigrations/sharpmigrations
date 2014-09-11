using NUnit.Framework;
using Sharp.Data;

namespace Sharp.Tests.MigratorProgram {
    public class ConfigurationTests {
        [Test]
        public void Can_set_connectionstring_by_name() {
            var migration = Create("-a", "Sharp.Tests.dll", "-n", "Oracle");
            migration.ParseConfig();
            Assert.AreEqual("Data Source=XE; User Id=sharp; Password=sharp;", SharpFactory.Default.ConnectionString);
            Assert.AreEqual("Oracle.ManagedDataAccess.Client", SharpFactory.Default.DataProviderName);
        }

        [Test]
        public void Can_set_connectionstring() {
            var migration = Create("-a", "Sharp.Tests.dll", 
                                   "-c", "Data Source=XE; User Id=sharp; Password=sharp;",
                                   "-p", "Oracle.ManagedDataAccess.Client"
                                   );
            migration.ParseConfig();
            Assert.AreEqual("Data Source=XE; User Id=sharp; Password=sharp;", SharpFactory.Default.ConnectionString);
            Assert.AreEqual("Oracle.ManagedDataAccess.Client", SharpFactory.Default.DataProviderName);
        }

        private Migrator.Migrator Create(params string[] args) {
            return new Migrator.Migrator(args);
        }
    }
}
