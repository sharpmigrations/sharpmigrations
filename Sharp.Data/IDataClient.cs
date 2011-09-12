using System;
using Sharp.Data.Filters;
using Sharp.Data.Fluent;
using Sharp.Data.Schema;

namespace Sharp.Data {
    
    public interface IDataClient : IDisposable  {

        IDatabase Database { get; }
        Dialect Dialect { get; }

        bool ThrowException { get; set; }

		FluentAdd Add { get; }
		FluentRemove Remove { get; }

		IFluentInsert Insert { get; }
		IFluentSelect Select { get; }
		IFluentUpdate Update { get; }
		IFluentDelete Delete { get; }
    	IFluentCount Count { get; }

    	void AddTable(string tableName, params FluentColumn[] columns);
        void AddColumn(string tableName, Column column);
        void AddForeignKey(string fkName, string table, string column, string referencingTable, string referencingColumn, OnDelete onDelete);
        void AddNamedPrimaryKey(string pkName, string tableName, params string[] columnNames);
        void AddPrimaryKey(string tableName, params string[] columnNames);
        void AddUniqueKey(string uniqueKeyName, string tableName, params string[] columnNames);
    	void AddIndex(string indexName, string tableName, params string[] columnNames);
        
        void RemoveColumn(string tableName, string columnName);
        void RemoveForeignKey(string foreigKeyName, string tableName);
        void RemoveTable(string tableName);
        void RemoveUniqueKey(string uniqueKeyName, string tableName);
		void RemoveIndex(string indexName);

        void Commit();
        void RollBack();
        void Close();

        ResultSet SelectSql(string table, string[] columns, Filter filter, OrderBy[] orderBys, int skip, int take);

        void InsertSql(string table, string[] columns, object[] values);
        object InsertReturningSql(string table, string columnToReturn, string[] columns, object[] values);
        int UpdateSql(string table, string[] columns, object[] values, Filter filter);
		int DeleteSql(string table, Filter filter);
    	int CountSql(string table, Filter filter);

    	bool TableExists(string table);
    }
}
