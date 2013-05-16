using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Schema;

namespace Sharp.Tests.Databases.Data {
    [Explicit]
    public abstract class DatabaseTests {

        protected IDataClient _dataClient;
        protected IDatabase _database;
        protected string TableFoo = "foo";

        protected virtual string GetParameterPrefix() {
            return ":";
        }
        
        [Test]
        public void Can_query_with_string_filter() {
            _dataClient.AddTable(TableFoo,
                                 Column.String("colString"),
                                 Column.Int32("colInt"));

            _dataClient.Insert.Into(TableFoo).Columns("colString", "colInt").Values("foo", 1);

            ResultSet resultSet = _database.Query("select colString from foo where colString = :name", In.Named("name", "foo"));
            Assert.AreEqual("foo", resultSet[0][0].ToString());
        }

        [Test]
        public void Can_query_with_string_and_int_filter() {
            _dataClient.AddTable(TableFoo,
                                 Column.String("colString"),
                                 Column.Int32("colInt"));

            _dataClient.Insert.Into(TableFoo).Columns("colString", "colInt").Values("foo", 1);

            ResultSet resultSet = _database.Query("select colString from foo where colString = :name and colInt = :id",
                In.Named("name", "foo"),
                In.Named("id", 1)
            );

            Assert.AreEqual("foo", resultSet[0][0].ToString());
        }

        [Test]
        public void Can_query_with_string_and_int_by_name_filter() {
            _dataClient.AddTable(TableFoo,
                                 Column.String("colString"),
                                 Column.Int32("colInt"));

            _dataClient.Insert.Into(TableFoo).Columns("colString", "colInt").Values("foo", 1);

            ResultSet resultSet = _database.Query("select colString from foo where colString = :name and colInt = :id",
                In.Named("id", 1),
                In.Named("name", "foo")
            );

            Assert.AreEqual("foo", resultSet[0][0].ToString());
        }

        [TearDown]
        public virtual void TearDown() {
            _database.RollBack();
            CleanTables();
            _database.Dispose();
        }

        public void CleanTables() {
            if (_dataClient.TableExists(TableFoo)) {
                _dataClient.Remove.Table(TableFoo);
            }
        }
    }
}
