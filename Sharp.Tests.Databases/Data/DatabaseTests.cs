using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Data.Databases.Oracle;
using Sharp.Data.Filters;
using Sharp.Data.Schema;
using Sharp.Data.Util;

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

            _database.ExecuteBulkSql("insert into " + TableFoo + " (colString, colInt) values (:v1,:v2)",
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
        public void Can_bulk_insert_with_nullable() {
            _dataClient.AddTable(TableFoo, Column.Decimal("colDecimal"));
            var v1s = new decimal?[] { 1, 2, 3, 4 };
            _database.ExecuteBulkSql("insert into " + TableFoo + " (colDecimal) values (:v1)",
                In.Named("v1", v1s)
            );
            ResultSet res = _dataClient.Select
                .AllColumns()
                .From(TableFoo)
                .AllRows();
            Assert.AreEqual(4, res.Count);
        }

        [Test]
        public void Can_bulk_insert_stored_procedure() {
            _dataClient.AddTable(TableFoo, Column.Int32("colInt"));
            try {
                _database.ExecuteSql("drop procedure pr_bulk");
            }
            catch {}
            _database.ExecuteSql("create or replace procedure pr_bulk(v_value in number) is begin insert into foo (colInt) values (v_value); end pr_bulk;");

            var v1s = new[] { 1, 2, 3, 4 };

            _database.ExecuteBulkStoredProcedure("pr_bulk", In.Named("v_value",v1s));
            ResultSet res = _dataClient.Select
                .AllColumns()
                .From(TableFoo)
                .AllRows();

            Assert.AreEqual(4, res.Count);
        }

        [Test]
        public void Can_bulk_insert_stored_procedure_with_nullable() {
            _dataClient.AddTable(TableFoo, Column.Decimal("colDecimal"));
            try {
                _database.ExecuteSql("drop procedure pr_bulk");
            }
            catch { }
            _database.ExecuteSql("create or replace procedure pr_bulk(v_value in float) is begin insert into foo (colDecimal) values (v_value); end pr_bulk;");

            var v1s = new decimal?[] { 1, 2, 3, 4, null };

            _database.ExecuteBulkStoredProcedure("pr_bulk", In.Named("v_value", v1s));
            ResultSet res = _dataClient.Select
                .AllColumns()
                .From(TableFoo)
                .AllRows();

            Assert.AreEqual(5, res.Count);
        }

        [Test]
        public void Can_bulk_insert_stored_procedure_with_nullable_and_dates() {
            _dataClient.AddTable(TableFoo, Column.Decimal("colDecimal"), Column.Date("colDate"));
            try {
                _database.ExecuteSql("drop procedure pr_bulk");
            }
            catch { }
            _database.ExecuteSql("create or replace procedure pr_bulk(v_value in float, v_date in date) is begin insert into foo (colDecimal, colDate) values (v_value, v_date); end pr_bulk;");

            var v1s = new decimal?[] { 1, 2, 3, 4, null };
            var v2s = new[] { DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now };

            _database.ExecuteBulkStoredProcedure("pr_bulk", In.Named("v_value", v1s), In.Named("v_date", v2s));
            ResultSet res = _dataClient.Select
                .AllColumns()
                .From(TableFoo)
                .AllRows();

            Assert.AreEqual(5, res.Count);
        }

        //[Test]
        //public void Test() {
        //    int num = 10;
        //    var tables = new string[num];
        //    var columns = new string[num];
        //    var ids = new int[num];
        //    var insertDates = new DateTime[num];
        //    var dates = new DateTime[num];
        //    var values = new int?[num];
        //    int index;
        //    for (index = 0; index < num; index++) {
        //        tables[index] = "tb_medidor_ene";
        //        columns[index] = "vl_eneat_del";
        //        ids[index] = 1;
        //        insertDates[index] = DateTime.Now;
        //        dates[index] = DateTime.Today.AddMinutes(index * 5);
        //        values[index] = index;
        //    }
        //    SharpFactory.Default.CreateDatabase("Data Source=//localhost/XE;User Id=W2E_PIM;Password=W2E_PIM", DataProviderNames.OracleManaged)
        //                .ExecuteStoredProcedureAndDispose("PR_DATAIN_INSERT_OR_UPDATE",
        //                    In.Named("pTable", tables),
        //                    In.Named("pColumn", columns),
        //                    In.Named("pId", ids),
        //                    In.Named("pDate", dates),
        //                    In.Named("pValue", values)
        //                );
        //}

        [TearDown]
        public virtual void TearDown() {
            _database.RollBack();
            CleanTables();
            _database.Dispose();
            _dataClient.Dispose();
            _database = null;
            _dataClient = null;
            typeof(OracleOdpProvider).GetField("_propOracleDbType", ReflectionHelper.NoRestrictions).SetValue(null, null);
            typeof(OracleOdpProvider).GetField("_oracleDbCommandType", ReflectionHelper.NoRestrictions).SetValue(null, null);
            GC.Collect();
        }

        public void CleanTables() {
            if (_dataClient.TableExists(TableFoo)) {
                _dataClient.Remove.Table(TableFoo);
            }
        }
    }
}
