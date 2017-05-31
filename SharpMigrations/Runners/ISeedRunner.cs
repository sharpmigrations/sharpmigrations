namespace SharpMigrations.Runners {
    public interface ISeedRunner {
        void Run(string seedName, string param = null, string migrationGroup = null);        
    }
}