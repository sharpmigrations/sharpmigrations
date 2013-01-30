using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Exceptions;

namespace Sharp.Tests.Databases.Data {
    [Explicit]
    public abstract class SpecificExceptionsTests {

        protected IDataClient _dataClient;
        protected IDatabase _database;

        [Test]
        [ExpectedException(typeof(TableNotFoundException))]
        public virtual void TableNotFoundException__DataClient__insert() {
            _dataClient.Insert.Into("footable").Columns("name").ValuesAnd("asdf").Return<int>("id");
        }

        [Test]
        [ExpectedException(typeof(TableNotFoundException))]
        public virtual void TableNotFoundException__Database__insert() {
            _database.ExecuteSql("insert into moo values (1)");
        }

        [Test]
        [ExpectedException(typeof(TableNotFoundException))]
        public virtual void TableNotFoundException__DataClient__update() {
            _dataClient.Update.Table("moo").SetColumns("moo").ToValues(1).AllRows();
        }

        [Test]
        [ExpectedException(typeof(TableNotFoundException))]
        public virtual void TableNotFoundException__Database__update() {
            _database.ExecuteSql("update moo set moo=1");
        }

        [Test]
        [ExpectedException(typeof(TableNotFoundException))]
        public virtual void TableNotFoundException__DataClient__select() {
            _dataClient.Select.AllColumns().From("moo").AllRows();
        }

        [Test]
        [ExpectedException(typeof(TableNotFoundException))]
        public virtual void TableNotFoundException__Database__select() {
            _database.ExecuteSql("select * from moo");
        }

        [TearDown]
        public virtual void TearDown() {
            _dataClient.RollBack();
            _dataClient.Dispose();
        }
    }
}
