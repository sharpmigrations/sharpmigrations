using System;
using System.Data;
using Moq;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Providers;

namespace Sharp.Tests.Data {

    [TestFixture]
    public class DatabaseTests {

        protected DataParameterCollectionFake _allParameters;
        protected Mock<IDbCommand> _cmd;
        protected Mock<IDbConnection> _connection;
        protected Database _db;
        protected Mock<IDbDataParameter> _parameter1, _parameter2;
        protected Mock<IDataProvider> _provider;
        protected string _sql = "foo";
        protected Mock<IDbTransaction> _transaction;
        protected Mock<Dialect> _dialect;

        [SetUp]
        public void SetUp() {
            _dialect = new Mock<Dialect>();
            _provider = new Mock<IDataProvider>();
            _connection = new Mock<IDbConnection>();
            _transaction = new Mock<IDbTransaction>();
            _cmd = new Mock<IDbCommand>();
            _parameter1 = new Mock<IDbDataParameter>();
            _parameter2 = new Mock<IDbDataParameter>();
            _parameter1.SetupAllProperties();
            _parameter2.SetupAllProperties();

            _allParameters = new DataParameterCollectionFake();

            _connection.Setup(p => p.BeginTransaction()).Returns(_transaction.Object);
            _connection.Setup(p => p.CreateCommand()).Returns(_cmd.Object);
            _provider.Setup(p => p.GetConnection()).Returns(_connection.Object);
            _provider.Setup(p => p.GetParameter())
                .Returns(() => _parameter1.Object)
                .Callback(() => _provider.Setup(p => p.GetParameter())
                                         .Returns(_parameter2.Object));

            _provider.Setup(p => p.GetParameter(It.IsAny<In>())).Returns(() => _parameter1.Object)
                                  .Callback(() => _provider.Setup(p => p.GetParameter())
                                  .Returns(_parameter2.Object));

            _cmd.SetupGet(p => p.Parameters).Returns(_allParameters);

            _db = new Database(_provider.Object, "");
        }

        [Test]
        public void Can_execute_sql_with_parameters() {
            _cmd.Setup(p => p.ExecuteNonQuery()).Returns(1);

            Assert.AreEqual(1, _db.ExecuteSql(_sql, "par1", "par2"));
            Assert.AreEqual(2, _cmd.Object.Parameters.Count);
        }

        [Test]
        public void Can_execute_sql_without_parameters() {
            _cmd.Setup(p => p.ExecuteNonQuery()).Returns(1);

            Assert.AreEqual(1, _db.ExecuteSql(_sql));
        }

        [Test]
        public void Commit_commits_the_transaction_and_closes() {
            _cmd.Setup(p => p.ExecuteNonQuery()).Returns(1);
            _db.ExecuteSql(_sql);
            _db.Commit();

            _transaction.Verify(p => p.Commit());
            _connection.Verify(p => p.Close());
        }

        [Test]
        public void Constructor_should_set_provider_and_connectionstring() {
            IDataProvider dataProvider = new Mock<IDataProvider>().Object;
            Database db = new Database(dataProvider, "foo");

            Assert.AreEqual("foo", db.ConnectionString);
            Assert.AreEqual(dataProvider, db.Provider);
        }

        [Test]
        public void Do_not_roolback_when_exception() {
            _cmd.Setup(p => p.ExecuteNonQuery()).Throws(new Exception("foo"));

            try {
                _db.ExecuteSql(_sql);
            }
            catch { }
            _transaction.Verify(p => p.Rollback(), Times.Never());
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void Throws_DatabaseException_when_some_exception_is_thrown() {
            var ex = new Exception("moo");
            _cmd.Setup(p => p.ExecuteNonQuery()).Throws(ex);
            _provider.Setup(x => x.CreateSpecificException(ex, _sql)).Returns(new DatabaseException("moo", ex, _sql));
            _db.ExecuteSql(_sql);
        }

    }
}