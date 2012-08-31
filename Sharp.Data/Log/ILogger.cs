namespace Sharp.Data.Log {
	public interface ILogger {
		void Info(string message);
		void Error(string message);
		void Warn(string message);
		void Debug(string message);
		bool IsDebugEnabled { get; }
	}
}