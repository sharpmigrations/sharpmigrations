namespace Sharp.Migrations {
    public interface IRunner {
        void Run(long targetVersion, string migrationGroup = null);
    }
}