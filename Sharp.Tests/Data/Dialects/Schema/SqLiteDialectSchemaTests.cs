using System;
using NUnit.Framework;
using Sharp.Data.Databases.SqLite;

namespace Sharp.Tests.Databases.SQLite {
	[TestFixture]
	public class SqLiteDialectSchemaTests : DialectSchemaTests {
		[SetUp]
		public void SetUp() {
			_dialect = new SqLiteDialect();
		}

		protected override string[] GetResultFor_Can_create_table_sql() {
			return new[] {"create table myTable (id integer not null primary key autoincrement, name varchar(255) not null)"};
		}

		protected override string[] GetResultFor_Can_drop_table() {
			return new [] {"drop table mytable"};
		}

		protected override string GetResultFor_Can_convert_column_to_sql__with_not_null() {
			return "col varchar(255) not null";
		}

		protected override string GetResultFor_Can_convert_column_to_sql__with_primary_key() {
			return "col varchar(255) not null primary key";
		}

		protected override string GetResultFor_Can_convert_column_to_sql__autoIncrement() {
			return "col integer not null primary key autoincrement";
		}

		protected override string GetResultFor_Can_convert_column_to_sql__autoIncrement_and_primary_key() {
			return "col integer not null primary key autoincrement";
		}

		protected override string GetResultFor_Can_convert_column_to_sql__default_value() {
			return "col varchar(255) null default ('some string')";
		}

		protected override string[] GetResultFor_Can_convert_column_to_values() {
			return new[] {
				"'foo'",
				"1",
				"true",
				"24.33",
				"2009-01-20T12:30:00"
			};
		}

	    protected override string GetResultFor_Can_add_comment_to_column() {
	        throw new NotImplementedException();
	    }
	}
}