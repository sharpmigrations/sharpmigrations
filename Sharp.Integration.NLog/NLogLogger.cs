using NLog;
using Sharp.Data.Log;
using LogManager = NLog.LogManager;

namespace Sharp.Integration.NLog {
    public class NLogLogger : ILogger {
        private readonly Logger _log;

        public bool IsDebugEnabled {
            get { return _log.IsDebugEnabled; }
        }

        public NLogLogger(string name) {
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
            if (_log.IsWarnEnabled) {
                _log.Warn(message);
            }
        }

        public void Debug(string message) {
            if (_log.IsDebugEnabled) {
                _log.Debug(message);
            }
        }
    }
}