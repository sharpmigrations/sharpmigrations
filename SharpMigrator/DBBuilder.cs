using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using SharpData;
using SharpData.Databases;

namespace SharpMigrator {
    public static class DbFinder {
        private static Dictionary<DbProviderType, SharpFactory> _factories = new Dictionary<DbProviderType, SharpFactory>();

        public static ISharpFactory GetSharpFactory(DbProviderType databaseProvider, string connectionString) {
            if (!_factories.ContainsKey(databaseProvider)) {
                switch (databaseProvider) {
                    case DbProviderType.OracleManaged:
                        _factories.Add(DbProviderType.OracleManaged,
                            new SharpFactory(new OracleClientFactory(), connectionString));
                        break;
                    case DbProviderType.MySql:
                        _factories.Add(DbProviderType.MySql,
                            new SharpFactory(new MySqlClientFactory(), connectionString));
                        break;
                    case DbProviderType.SqlServer:
                        _factories.Add(DbProviderType.SqlServer,
                            new SharpFactory(SqlClientFactory.Instance, connectionString));
                        break;
                    case DbProviderType.SqLite:
                        _factories.Add(DbProviderType.SqLite,
                            new SharpFactory(SQLiteFactory.Instance, connectionString));
                        break;
                    case DbProviderType.PostgreSql:
                        _factories.Add(DbProviderType.PostgreSql,
                            new SharpFactory(NpgsqlFactory.Instance, connectionString));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(databaseProvider), databaseProvider, null);
                }
            }
            return _factories[databaseProvider];
        }
    }
}