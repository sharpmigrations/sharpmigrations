using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Exceptions;
using Sharp.Data.Schema;

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


        private void CreateTableFoo() {
            _dataClient.Add.Table("foo").WithColumns(Column.Int32("id").AsPrimaryKey());
        }

        [Test]
        [ExpectedException(typeof(UniqueConstraintException))]
        public virtual void UniqueConstraintException__DataClient__insert() {
            CreateTableFoo();
            _dataClient.Insert.Into("foo").Columns("id").Values(1);
            _dataClient.Insert.Into("foo").Columns("id").Values(1);
        }

        [Test]
        [ExpectedException(typeof(UniqueConstraintException))]
        public virtual void UniqueConstraintException__Database__insert() {
             CreateTableFoo();
            _database.ExecuteSql("insert into foo values (1)");
            _database.ExecuteSql("insert into foo values (1)");
        }

        [Test]
        [ExpectedException(typeof(UniqueConstraintException))]
        public virtual void UniqueConstraintException__DataClient__update() {
             CreateTableFoo();
            _dataClient.Insert.Into("foo").Columns("id").Values(1);
            _dataClient.Insert.Into("foo").Columns("id").Values(2);
            _dataClient.Update.Table("foo").SetColumns("id").ToValues(1).AllRows();
        }

        [Test]
        [ExpectedException(typeof(UniqueConstraintException))]
        public virtual void UniqueConstraintException__Database__update() {
             CreateTableFoo();
            _database.ExecuteSql("insert into foo values (1)");
            _database.ExecuteSql("insert into foo values (2)");
            _database.ExecuteSql("update foo set id=1");
        }

        [TearDown]
        public virtual void TearDown() {
            if (_dataClient.TableExists("foo")) {
                _dataClient.RemoveTable("foo");                
            }
            _dataClient.RollBack();
            _dataClient.Dispose();
        }
    }
}
