namespace Sharp.Data.Fluent {
    public interface IFluentRename {
        IRenameTableTo Table(string tableName);
        IRenameColumnOfTable Column(string columnName);
    }
}