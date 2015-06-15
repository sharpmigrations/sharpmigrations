using System;
using System.Data;

namespace Sharp.Data.Databases.PostgreSql {
    public class PostgreSqlDbTypesDialect {
        public static string GetDbTypeString(DbType type, int precision) {
            switch (type) {
                case DbType.AnsiString:
                case DbType.String:
                    if (precision <= 0)
                        return "VARCHAR(255)";
                    if (precision < 10485760)
                        return String.Format("VARCHAR({0})", precision);
                    return "TEXT";
                case DbType.Binary:
                    return "BYTEA";
                case DbType.Boolean:
                    return "BOOL";
                case DbType.Currency:
                    return "MONEY";
                case DbType.Date:
                case DbType.DateTime:
                    return "TIMESTAMP";
                case DbType.Decimal:
                    return precision <= 0 ? "NUMERIC(19,5)" : String.Format("NUMERIC(19,{0})", precision);
                case DbType.Double:
                    return "FLOAT8";
                case DbType.Guid:
                    return "UUID";
                case DbType.Int16:
                    return "SMALLINT";
                case DbType.Int32:
                    return "INTEGER";
                case DbType.Int64:
                    return "BIGINT";
                case DbType.Single:
                    return "FLOAT4";
                case DbType.Time:
                    return "TIME";
                case DbType.AnsiStringFixedLength:
                case DbType.StringFixedLength:
                    return precision <= 0 ? "CHAR(255)" : String.Format("CHAR({0})", precision);
                case DbType.Xml:
                    return "XML";
                case DbType.DateTimeOffset:
                    return "TIMESTAMPTZ";
                default:
                    throw new DataTypeNotAvailableException(String.Format("The type {0} is no available for postgreSql", type));
            }
        }
    }
}