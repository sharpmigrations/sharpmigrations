using System;
using NUnit.Framework;
using Sharp.Data.Databases.PostgreSql;

namespace  Sharp.Tests.Databases.PostgreSql {
    class PostgreSqlDialectSchemaTests : DialectSchemaTests {
        [SetUp]
        public void SetUp() {
            _dialect = new PostgreSqlDialect();
        }

        protected override string[] GetResultFor_Can_create_table_sql() {
            return new[] { "create table myTable (id integer not null, name varchar(255) not null)" };
        }

        protected override string[] GetResultFor_Can_drop_table() {
            throw new NotImplementedException();
        }

        protected override string GetResultFor_Can_convert_column_to_sql__with_not_null() {
            throw new NotImplementedException();
        }

        protected override string GetResultFor_Can_convert_column_to_sql__with_primary_key() {
            throw new NotImplementedException();
        }

        protected override string GetResultFor_Can_convert_column_to_sql__autoIncrement() {
            throw new NotImplementedException();
        }

        protected override string GetResultFor_Can_convert_column_to_sql__autoIncrement_and_primary_key() {
            throw new NotImplementedException();
        }

        protected override string GetResultFor_Can_convert_column_to_sql__default_value() {
            throw new NotImplementedException();
        }

        protected override string[] GetResultFor_Can_convert_column_to_values() {
            throw new NotImplementedException();
        }

        protected override string GetResultFor_Can_add_comment_to_column() {
            throw new NotImplementedException();
        }
    }
}
