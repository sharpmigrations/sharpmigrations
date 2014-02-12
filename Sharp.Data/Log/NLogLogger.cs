using System;
using System.Diagnostics;
using System.Reflection;

namespace Sharp.Data.Log {
    public class NLogLogger : ISharpLogger {
        private object _logger;

        private static MethodInfo ErrorMethod, FatalMethod, DebugMethod, InfoMethod, WarnMethod;
        private static MethodInfo ErrorExceptionMethod, FatalExceptionMethod, DebugExceptionMethod, InfoExceptionMethod, WarnExceptionMethod;
        private static PropertyInfo IsErrorMethod, IsFatalMethod, IsDebugMethod, IsInfoMethod, IsWarnMethod;

        public NLogLogger(object logger) {
            _logger = logger;
            if (DebugMethod == null) {
                CacheMethods();
            }
        }

        private void CacheMethods() {
            Type loggerType = _logger.GetType();

            ErrorMethod = loggerType.GetMethod("Error", new[] {typeof (string)});
            FatalMethod = loggerType.GetMethod("Fatal", new[] {typeof (string)});
            DebugMethod = loggerType.GetMethod("Debug", new[] {typeof (string)});
            InfoMethod = loggerType.GetMethod("Info", new[] {typeof (string)});
            WarnMethod = loggerType.GetMethod("Warn", new[] {typeof (string)});

            ErrorExceptionMethod = loggerType.GetMethod("DebugException", new[] { typeof(string), typeof(Exception) });
            FatalExceptionMethod = loggerType.GetMethod("FatalException", new[] { typeof(string), typeof(Exception) });
            DebugExceptionMethod = loggerType.GetMethod("DebugException", new[] { typeof(string), typeof(Exception) });
            InfoExceptionMethod = loggerType.GetMethod("InfoException", new[] { typeof(string), typeof(Exception) });
            WarnExceptionMethod = loggerType.GetMethod("WarnException", new[] { typeof(string), typeof(Exception) });


            IsErrorMethod = loggerType.GetProperty("IsErrorEnabled", typeof(bool));
            IsFatalMethod = loggerType.GetProperty("IsFatalEnabled", typeof(bool));
            IsDebugMethod = loggerType.GetProperty("IsDebugEnabled", typeof(bool));
            IsInfoMethod = loggerType.GetProperty("IsInfoEnabled", typeof(bool));
            IsWarnMethod = loggerType.GetProperty("IsWarnEnabled", typeof(bool));
        }

        public bool IsErrorEnabled {
            get {
                return (bool) IsErrorMethod.GetValue(_logger, null);
            }
        }

        public bool IsDebugEnabled {
            get {
                return (bool)IsDebugMethod.GetValue(_logger, null);
            }
        }

        public bool IsFatalEnabled {
            get {
                return (bool)IsFatalMethod.GetValue(_logger, null);
            }
        }

        public bool IsInfoEnabled {
            get {
                return (bool)IsInfoMethod.GetValue(_logger, null);
            }
        }

        public bool IsWarnEnabled {
            get {
                return (bool)IsWarnMethod.GetValue(_logger, null);
            }
        }
       
        public void Error(object message) {
            if (!IsErrorEnabled) return;
            ErrorMethod.Invoke(_logger, new[] { message });
        }

        public void Error(object message, Exception exception) {
            if (!IsErrorEnabled) return;
            ErrorExceptionMethod.Invoke(_logger, new[] { message, exception });
        }

        public void ErrorFormat(string format, params object[] args) {
            if (!IsErrorEnabled) return;
            string message = String.Format(format, args);
            ErrorMethod.Invoke(_logger, new object[] { message });
        }

        public void Fatal(object message) {
            if (!IsFatalEnabled) return;
            FatalMethod.Invoke(_logger, new[] { message });
        }

        public void Fatal(object message, Exception exception) {
            if (!IsFatalEnabled) return;
            FatalExceptionMethod.Invoke(_logger, new[] { message, exception });
        }

        public void Debug(object message) {
            if (!IsDebugEnabled) return;
            DebugMethod.Invoke(_logger, new[] { message });
        }

        public void Debug(object message, Exception exception) {
            if (!IsDebugEnabled) return;
            DebugExceptionMethod.Invoke(_logger, new[] { message, exception });
        }

        public void DebugFormat(string format, params object[] args) {
            if (!IsDebugEnabled) return;
            string message = String.Format(format, args);
            DebugMethod.Invoke(_logger, new object[] { message });
        }

        public void Info(object message) {
            if (!IsInfoEnabled) return;
            InfoMethod.Invoke(_logger, new[] { message });
        }

        public void Info(object message, Exception exception) {
            if (!IsInfoEnabled) return;
            InfoExceptionMethod.Invoke(_logger, new[] { message, exception });
        }

        public void InfoFormat(string format, params object[] args) {
            if (!IsInfoEnabled) return;
            string message = String.Format(format, args);
            InfoMethod.Invoke(_logger, new object[] { message });
        }

        public void Warn(object message) {
            if (!IsWarnEnabled) return;
            WarnMethod.Invoke(_logger, new[] { message });
        }

        public void Warn(object message, Exception exception) {
            if (!IsWarnEnabled) return;
            WarnExceptionMethod.Invoke(_logger, new[] { message, exception });
        }

        public void WarnFormat(string format, params object[] args) {
            if (!IsWarnEnabled) return;
            string message = String.Format(format, args);
            WarnMethod.Invoke(_logger, new object[] { message });
        }
    }
}