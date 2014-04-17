using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Filters;
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

        [Test]
        public void Can_bulk_insert() {
            _dataClient.AddTable(TableFoo,
                                 Column.String("colString"),
                                 Column.Int32("colInt"));
            var v1s = new[] {"1", "2", "3", "4"};
            var v2s = new[] { 1, 2, 3, 4 };

            _database.ExecuteSql("insert into " + TableFoo + " (colString, colInt) values (:v1,:v2)",
                In.Named("v1", v1s), 
                In.Named("v2", v2s)
            );
            ResultSet res = _dataClient.Select
                .AllColumns()
                .From(TableFoo)
                .AllRows();
                                        
            Assert.AreEqual(4, res.Count);
        }

        [Test]
        public void Can_bulk_insert_stored_procedure() {
            _dataClient.AddTable(TableFoo,
                                 Column.Int32("colInt"));
            try {
                _database.ExecuteSql("drop procedure pr_bulk");
            }
            catch {}
            _database.ExecuteSql("create or replace procedure pr_bulk(v_value in number) is begin insert into foo (colInt) values (v_value); end pr_bulk;");

            var v1s = new[] { 1, 2, 3, 4 };

            _database.ExecuteStoredProcedure("pr_bulk", In.Named("v_value",v1s));
            ResultSet res = _dataClient.Select
                .AllColumns()
                .From(TableFoo)
                .AllRows();

            Assert.AreEqual(4, res.Count);
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
