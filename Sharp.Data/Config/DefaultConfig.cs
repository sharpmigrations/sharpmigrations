namespace Sharp.Data.Config {
	public static class DefaultConfig {
		public static string ConnectionString;
		public static string DatabaseProvider;
		public static bool IgnoreDialectNotSupportedActions;
		public static string AppName = "Default";

		public static void LoadFromAppConfig() {
			ConfigFromAppConfig.LoadConfig();
		}
	}
}