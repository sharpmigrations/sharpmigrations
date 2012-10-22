namespace Sharp.Data.Log {
	public static class LogManager {

		private static ILoggerFactory _factory = new EmptyLoggerFactory();

		public static void Configure(ILoggerFactory factory) {
			_factory = factory;
		}

		public static ILogger GetLogger(string name) {
			return _factory.GetLogger(name);
		}

	}
}