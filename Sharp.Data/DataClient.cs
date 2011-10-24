using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Sharp.Data.Databases;
using Sharp.Data.Filters;
using Sharp.Data.Fluent;
using Sharp.Data.Schema;

namespace Sharp.Data {

    public class DataClient : IDataClient {

		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        public IDatabase Database { get; set; }
        public Dialect Dialect { get; protected set; }
        
        public bool ThrowException { get; set; }

        public DataClient(IDatabase database, Dialect dialect) {
            Database = database;
            Dialect = dialect;
            
			ThrowException = true;
        }

		public FluentAdd Add {
			get { return new FluentAdd(this); }
		}

		public FluentRemove Remove {
			get { return new FluentRemove(this); }
		}

		public IFluentInsert Insert {
			get { return new FluentInsert(this); }
		}

		public IFluentUpdate Update {
			get { return new FluentUpdate(this); }
		}

		public IFluentSelect Select {
			get { return new FluentSelect(this); }
		}

		public IFluentDelete Delete {
			get { return new FluentDelete(this); }
		}

    	public IFluentCount Count {
			get { return new FluentCount(this); }
    	}

    	public virtual void AddTable(string tableName, params FluentColumn[] columns) {
            Table table = new Table(tableName);
            foreach (FluentColumn fcol in columns) {
                table.Columns.Add(fcol.Object);
            }

            string[] sqls = Dialect.GetCreateTableSqls(table);
            
			ExecuteSqls(sqls);
        }

    	private void ExecuteSqls(string[] sqls) {
    		foreach (string sql in sqls) {
    			Database.ExecuteSql(sql);
    		}
    	}

    	public virtual void RemoveTable(string tableName) {
            string[] sqls = Dialect.GetDropTableSqls(tableName);
			ExecuteSqls(sqls);
        }

        public virtual void AddPrimaryKey(string tableName, params string[] columnNames) {
            string sql = Dialect.GetPrimaryKeySql("pk_" + tableName, tableName, columnNames);
            Database.ExecuteSql(sql);
        }

        public virtual void AddNamedPrimaryKey(string pkName, string tableName, params string[] columnNames) {
            string sql = Dialect.GetPrimaryKeySql(pkName, tableName, columnNames);
            Database.ExecuteSql(sql);
        }

        public virtual void AddForeignKey(string fkName, string table, string column, string referencingTable,
                                  string referencingColumn, OnDelete onDelete) {
            string sql = Dialect.GetForeignKeySql(fkName, table, column, referencingTable, referencingColumn, onDelete);
            Database.ExecuteSql(sql);
        }

        public virtual void RemoveForeignKey(string foreigKeyName, string tableName) {
            string sql = Dialect.GetDropForeignKeySql(foreigKeyName, tableName);
            Database.ExecuteSql(sql);
        }

        public virtual void AddUniqueKey(string uniqueKeyName, string tableName, params string[] columnNames) {
            string sql = Dialect.GetUniqueKeySql(uniqueKeyName, tableName, columnNames);
            Database.ExecuteSql(sql);
        }

    	public virtual void RemoveUniqueKey(string uniqueKeyName, string tableName) {
            string sql = Dialect.GetDropUniqueKeySql(uniqueKeyName, tableName);
            Database.ExecuteSql(sql);
        }

    	public void AddIndex(string indexName, string tableName, params string[] columnNames) {
    		string sql = Dialect.GetCreateIndexSql(indexName, tableName, columnNames);
    		Database.ExecuteSql(sql);
    	}

    	public void RemoveIndex(string indexName) {
    		string sql = Dialect.GetDropIndexSql(indexName);
			Database.ExecuteSql(sql);
    	}

    	public virtual void AddColumn(string tableName, Column column) {
            string sql = Dialect.GetAddColumnSql(tableName, column);
            Database.ExecuteSql(sql);
        }

        public virtual void RemoveColumn(string tableName, string columnName) {
            string sql = Dialect.GetDropColumnSql(tableName, columnName);
            Database.ExecuteSql(sql);
        }

        public virtual ResultSet SelectSql(string[] tables, string[] columns, Filter filter, OrderBy[] orderBys, int skip, int take) {
            SelectBuilder selectBuilder = new SelectBuilder(Dialect, tables, columns);
            selectBuilder.Filter = filter;
            selectBuilder.OrderBys = orderBys;
            selectBuilder.Skip = skip;
            selectBuilder.Take = take;

            string sql = selectBuilder.Build();
            if (selectBuilder.HasFilter) {
                return Database.Query(sql, selectBuilder.Parameters);
            }
            return Database.Query(sql);
        }

        public virtual void InsertSql(string table, string[] columns, object[] values) {
            if (values == null) {
                values = new object[columns.Length];
            }
            string sql = Dialect.GetInsertSql(table, columns, values);
            Database.ExecuteSql(sql, Dialect.ConvertToNamedParameters(values));
        }

        public virtual object InsertReturningSql(string table, string columnToReturn, string[] columns, object[] values) {

            Out returningPar = new Out {Name = "returning_" + columnToReturn, Size = 4000};

            string retSql = Dialect.GetInsertReturningColumnSql(table, columns, values, columnToReturn, returningPar.Name);

            object[] pars = Dialect.ConvertToNamedParameters(values);
            List<object> listPars = pars.ToList();
            listPars.Add(returningPar);

            Database.ExecuteSql(retSql, listPars.ToArray());

            return returningPar.Value;
        }

        public virtual int UpdateSql(string table, string[] columns, object[] values, Filter filter) {
            string sql = Dialect.GetUpdateSql(table, columns, values);

            In[] parameters = Dialect.ConvertToNamedParameters(values);

            if (filter != null) {
                string whereSql = Dialect.GetWhereSql(filter, parameters.Count());

            	object[] pars = filter.GetAllValueParameters();

                In[] filterParameters = Dialect.ConvertToNamedParameters(parameters.Count(), pars);

                parameters = parameters.Concat(filterParameters).ToArray();

                sql = sql + " " + whereSql;
            }

            return Database.ExecuteSql(sql, parameters);
        }

        public virtual int DeleteSql(string table, Filter filter) {
            string sql = Dialect.GetDeleteSql(table);

            if (filter != null) {
                string whereSql = Dialect.GetWhereSql(filter, 0);

				object[] pars = filter.GetAllValueParameters();

                In[] parameters = Dialect.ConvertToNamedParameters(0, pars);

                return Database.ExecuteSql(sql + " " + whereSql, parameters);
            }

            return Database.ExecuteSql(sql);
        }

		public virtual int CountSql(string table, Filter filter) {
			string sql = Dialect.GetCountSql(table);
			object obj = null;

			if (filter != null) {
				string whereSql = Dialect.GetWhereSql(filter, 0);

				object[] pars = filter.GetAllValueParameters();

				In[] parameters = Dialect.ConvertToNamedParameters(0, pars);

				obj = Database.QueryScalar(sql + " " + whereSql, parameters);
				return Convert.ToInt32(obj);
			}

			obj = Database.QueryScalar(sql);
			return Convert.ToInt32(obj);
		}

    	public bool TableExists(string table) {
    		string sql = Dialect.GetTableExistsSql(table);

    		return Convert.ToInt32(Database.QueryScalar(sql)) > 0;
    	}

    	public void Commit() {
            if (Database != null) {
                Database.Commit();
            }
        }

        public void RollBack() {
            if (Database != null) {
                Database.RollBack();
            }
        }

        public void Close() {
            if (Database != null) {
                Database.Close();
            }
        }

    	public void Dispose() {
    		Close();
		}
    }
}