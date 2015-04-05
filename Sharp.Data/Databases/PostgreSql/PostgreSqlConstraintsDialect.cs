using System;
using Sharp.Data.Schema;
using Sharp.Util;

namespace Sharp.Data.Databases.PostgreSql {
    public class PostgreSqlConstraintsDialect {
        public string GetForeignKeySql(string fkName, string table, string column, string referencingTable, string referencingColumn, OnDelete onDelete) {
            string onDeleteSql;
            switch (onDelete) {
                case OnDelete.Cascade:
                    onDeleteSql = "ON DELETE CASCADE";
                    break;
                case OnDelete.SetNull:
                    onDeleteSql = "ON DELETE SET NULL";
                    break;
                default:
                    onDeleteSql = "";
                    break;
            }

            return String.Format("ALTER TABLE {0} ADD CONSTRAINT {1} FOREIGN KEY ({2}) REFERENCES {3} ({4}) {5}",
                             table,
                             fkName,
                             column,
                             referencingTable,
                             referencingColumn,
                             onDeleteSql);
        }

        public string GetUniqueKeySql(string ukName, string table, string[] columnNames) {
            return String.Format("ALTER TABLE {0} ADD CONSTRAINT {1} UNIQUE ({2})", table, ukName, StringHelper.Implode(columnNames, ","));
        }

        public string GetDropUniqueKeySql(string uniqueKeyName, string tableName) {
            return String.Format("ALTER TABLE {0} DROP CONSTRAINT {1}", tableName, uniqueKeyName);
        }
    }
}