namespace Sharp.Migrations {
	public interface IVersionRepository {
        string MigrationGroup { get; set; }

		int GetCurrentVersion();
		void UpdateVersion(int version);
	}
}