using log4net;
using Sharp.Data.Log;
using LogManager = log4net.LogManager;

namespace Sharp.Integration.Log4net {
	public class Log4NetLogger : ILogger {
		private readonly ILog _log;

		public bool IsDebugEnabled {
			get { return _log.IsDebugEnabled; }
		}

		public Log4NetLogger(string name) {
			_log = LogManager.GetLogger(name);
		}

		public void Info(string message) {
			if (_log.IsInfoEnabled) {
				_log.Info(message);
			}
		}

		public void Error(string message) {
			if (_log.IsErrorEnabled) {
				_log.Error(message);
			}
		}

		public void Warn(string message) {
			if(_log.IsWarnEnabled) {
				_log.Warn(message);
			}
		}

		public void Debug(string message) {
			if(_log.IsDebugEnabled) {
				_log.Debug(message);
			}
		}
	}
}