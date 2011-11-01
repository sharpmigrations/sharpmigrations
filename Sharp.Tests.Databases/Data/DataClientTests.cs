using System;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Filters;
using Sharp.Data.Schema;

namespace Sharp.Tests.Databases.Data {

    [Explicit]
    public abstract class DataClientTests {
        protected IDataClient _dataClient;
        protected string tableFoo = "foo";       
       

		[Test]
		public void Should_return_false_if_table_doesnt_exist() {
			Assert.IsFalse(_dataClient.TableExists("foo"));
		}

		[Test]
		public void Should_return_true_if_table_exists() {
			CreateTableFoo();
			Assert.IsTrue(_dataClient.TableExists("foo"));
		}

    	[TearDown]
        public virtual void TearDown() {
            DropTable(tableFoo);
            DropTable("bar");
            DropTable("foobar");
            DropTable("footable");
            _dataClient.RollBack();
            _dataClient.Dispose();
        }

        protected void CreateTableFoo() {
            _dataClient.AddTable(tableFoo,
                                 Column.Int32("id").AsPrimaryKey(),
                                 Column.String("name")
                );
        }

        protected void PopulateTableFoo() {
            _dataClient.Insert.Into(tableFoo).Columns("id", "name")
                .Values(1, "v1")
                .Values(2, "v2")
                .Values(3, "v3");
        }

        protected void CreateTableBar() {
            _dataClient.AddTable("bar",
                                 Column.Int32("id").AsPrimaryKey(),
                                 Column.Int32("id_foo1").NotNull(),
                                 Column.Int32("id_foo2").NotNull(),
                                 Column.String("name")
                );
        }


        protected void DropTable(string tableName) {
            try {
                _dataClient.RemoveTable(tableName);
            }
            catch {}
        }
    }
}