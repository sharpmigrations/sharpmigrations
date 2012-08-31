namespace Sharp.Data.Log {
	public interface ILoggerFactory {
		ILogger GetLogger(string name);
	}
}