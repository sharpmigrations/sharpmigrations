namespace Sharp.Data.Log {
	internal class EmptyLoggerFactory :ILoggerFactory{
		public ILogger GetLogger(string name) {
			return new EmptyLogger();
		}
	}
}