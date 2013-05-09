using Sharp.Data.Schema;

namespace Sharp.Data.Fluent {
    public interface IFluentAdd {
        IAddColumnToTable Column(FluentColumn column);
        IAddPrimaryKeyOnColumns PrimaryKey(string primaryKeyName);
        IAddForeignKeyOnColumn ForeignKey(string foreignKeyName);
        IAddUniqueKeyOnColumns UniqueKey(string uniqueKeyName);
        DataClientAddIndexKey IndexKey(string indexKeyName);
        IAddTableWithColumns Table(string tableName);
        IAddCommentColumnOrTable Comment(string comment);
    }
}