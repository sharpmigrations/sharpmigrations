using NUnit.Framework;
using Sharp.Data.Databases.PostgreSql;

namespace Sharp.Tests.Databases.PostgreSql {
    public class PostgreSqlDialectSchemaTests : DialectSchemaTests {
        [SetUp]
        public void SetUp() {
            _dialect = new PostgreSqlDialect();
        }

        protected override string[] GetResultFor_Can_create_table_sql() {
            return new[] { "create table myTable (id integer not null, name varchar(255) not null)",
				"create sequence seq_mytable increment 1 minvalue 1 maxvalue 9223372036854775807 start 1 cache 1",
				"alter table myTable alter column id set default nextval(\"seq_mytable\"::regclass)",
				"alter table mytable add constraint pk_mytable primary key (id)"};
        }

        protected override string[] GetResultFor_Can_drop_table() {
            return new[] { "drop table mytable cascade" };
        }

        protected override string GetResultFor_Can_convert_column_to_sql__with_not_null() {
            return "col varchar(255) not null";
        }

        protected override string GetResultFor_Can_convert_column_to_sql__with_primary_key() {
            return "col varchar(255) not null";
        }

        protected override string GetResultFor_Can_convert_column_to_sql__autoIncrement() {
            return "col integer not null";
        }

        protected override string GetResultFor_Can_convert_column_to_sql__autoIncrement_and_primary_key() {
            return "col integer not null";
        }

        protected override string GetResultFor_Can_convert_column_to_sql__default_value() {
            return "col varchar(255) default 'some string' null";
        }

        protected override string[] GetResultFor_Can_convert_column_to_values() {
            return new[] {
				"'foo'",
				"1",
				"true",
				"24.33",
				"'2009-01-20T12:30:00'"
			};
        }

        protected override string GetResultFor_Can_add_comment_to_column() {
            return "COMMENT ON column myTable.col1 is 'this is a comment'";
        }

        protected override string GetResutFor_Can_drop_index_sql() {
            return "drop index indexName";
        }
    }
}
