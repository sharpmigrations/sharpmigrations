using System;
using NUnit.Framework;
using Sharp.Data.Databases.MySql;

namespace Sharp.Tests.Databases.Mysql {
    [TestFixture]
    public class MySqlDialectTests : DialectTests {
    	
		[SetUp]
    	public void SetUp() {
    		_dialect = new MySqlDialect();
    	}

    	protected override string GetResultFor_Can_create_check_if_table_exists_sql() {
    		return "select count(TABLE_NAME) from INFORMATION_SCHEMA where TABLE_NAME = 'mytable'";
		}

    	protected override string[] GetResultFor_Can_create_table_sql() {
			return new[] { "create table myTable (id int not null auto_increment, name VARCHAR(255) not null, primary key(id))" };
    	}

    	protected override string[] GetResultFor_Can_drop_table() {
			return new[] { "drop table myTable" };
    	}

    	protected override string GetResultFor_Can_convert_column_to_sql__with_not_null() {
            return "col varchar(255) not null";
        }

    	protected override string GetResultFor_Can_convert_column_to_sql__with_primary_key() {
			//primary key is defined after all columns when creating table
            return "col varchar(255) not null";
        }

    	protected override string GetResultFor_Can_convert_column_to_sql__autoIncrement() {
			return "col int not null auto_increment";
        }

    	protected override string GetResultFor_Can_convert_column_to_sql__autoIncrement_and_primary_key() {
			//primary key is defined after all columns when creating table
			return "col int not null auto_increment";
    	}

    	protected override string GetResultFor_Can_convert_column_to_sql__default_value() {
            return "col varchar(255) null default 'some string'";
        }

    	protected override string[] GetResultFor_Can_convert_column_to_values() {
                return new[] {
                     "'foo'",
                     "1",
                     "1",
                     "24.33",
                     "'2009-01-20 12:30:00'"
                };
        }

    	protected override string GetResultFor_Can_generate_count_sql() {
    		return "SELECT COUNT(*) FROM myTable";
    	}

    	protected override string GetResultFor_Can_generate_select_sql_with_pagination(int skip, int to) {
    		throw new NotImplementedException();
    	}
    }
}