using Sharp.Data.Fluent;

namespace Sharp.Migrations {
    public abstract class DataMigration : Migration {

        protected FluentDelete Delete { get { return new FluentDelete(this.DataClient);}}

        protected FluentInsert Insert { get { return new FluentInsert(this.DataClient);}}
            
        protected FluentSelect Select { get { return new FluentSelect(this.DataClient);}}

        protected FluentUpdate Update { get { return new FluentUpdate(this.DataClient);}}
    }
}