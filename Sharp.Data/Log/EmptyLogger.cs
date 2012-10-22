namespace Sharp.Data.Log {
	internal class EmptyLogger : ILogger {
		public void Info(string message) { }
		public void Error(string message) { }
		public void Warn(string message) { }
		public void Debug(string message) { }

		public bool IsDebugEnabled {
			get { return false; }
		}
	}
}