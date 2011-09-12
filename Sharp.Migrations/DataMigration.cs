using Sharp.Data.Fluent;

namespace Sharp.Migrations {
    public abstract class DataMigration : Migration {

        protected IFluentDelete Delete { get { return new FluentDelete(DataClient);}}

        protected IFluentInsert Insert { get { return new FluentInsert(DataClient);}}
            
        protected IFluentSelect Select { get { return new FluentSelect(DataClient);}}

        protected IFluentUpdate Update { get { return new FluentUpdate(DataClient);}}
    }
}