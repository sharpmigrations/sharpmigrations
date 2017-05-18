using Sharp.Data;
using Xunit;

namespace SharpMigrator.Tests {
    public class ConfigurationTests {
        //[Fact]
        //public void Can_set_connectionstring_by_name() {
        //    var migrator = Create("-a", "SharpMigrations.Tests.dll", "-n", "Oracle");
        //    migrator.ParseConfig();
            
        //    Assert.Equal("Data Source=XE; User Id=sharp; Password=sharp;", SharpFactory.Default.ConnectionString);
        //    Assert.Equal("Oracle.ManagedDataAccess.Client", SharpFactory.Default.DataProviderName);
        //}

        //[Fact]
        //public void Can_set_connectionstring() {
        //    var migration = Create("-a", "SharpMigrations.Tests.dll", 
        //                           "-c", "Data Source=XE; User Id=sharp; Password=sharp;",
        //                           "-p", "Oracle.ManagedDataAccess.Client"
        //                           );
        //    migration.ParseConfig();
        //    Assert.Equal("Data Source=XE; User Id=sharp; Password=sharp;", SharpFactory.Default.ConnectionString);
        //    Assert.Equal("Oracle.ManagedDataAccess.Client", SharpFactory.Default.DataProviderName);
        //}

        //private Migrator Create(params string[] args) {
        //    return new Migrator(args);
        //}
    }
}
