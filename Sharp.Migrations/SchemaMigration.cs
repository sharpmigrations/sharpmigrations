using Sharp.Data.Fluent;

namespace Sharp.Migrations {
    public abstract class SchemaMigration : Migration {

        private FluentAdd _schemaMigrationAdd;
        private FluentRemove _schemaMigrationRemove;
        internal bool ThrowException {
            set { DataClient.ThrowException = value; }
        }

        public FluentAdd Add {
            get {
                if (_schemaMigrationAdd == null) _schemaMigrationAdd = new FluentAdd(DataClient);
                return _schemaMigrationAdd;
            }
        }

        public FluentRemove Remove {
            get {
                if (_schemaMigrationRemove == null) _schemaMigrationRemove = new FluentRemove(DataClient);
                return _schemaMigrationRemove;
            }
        }
    }
}