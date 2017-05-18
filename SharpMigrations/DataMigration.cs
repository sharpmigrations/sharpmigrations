using SharpData.Fluent;

namespace SharpMigrations {
    public abstract class DataMigration : Migration {

        protected IFluentDelete Delete => new FluentDelete(DataClient);
        protected IFluentInsert Insert => new FluentInsert(DataClient);
        protected IFluentSelect Select => new FluentSelect(DataClient);
        protected IFluentUpdate Update => new FluentUpdate(DataClient);
    }
}