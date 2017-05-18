namespace SharpMigrations {
    public interface IRunner {
        void Run(long targetVersion);
    }
}