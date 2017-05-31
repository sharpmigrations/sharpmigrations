namespace SharpMigrations.Runners {
    public interface IRunner {
        void Run(long targetVersion);
    }
}