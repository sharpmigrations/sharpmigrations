using System;
using System.Linq.Expressions;

namespace Sharp.Data.Log {
    public class Log4NetLogger : ISharpLogger {
        private static readonly Type ILogType = Type.GetType("log4net.ILog, log4net");
        private static readonly Func<object, bool> IsErrorEnabledDelegate;
        private static readonly Func<object, bool> IsFatalEnabledDelegate;
        private static readonly Func<object, bool> IsDebugEnabledDelegate;
        private static readonly Func<object, bool> IsInfoEnabledDelegate;
        private static readonly Func<object, bool> IsWarnEnabledDelegate;

        private static readonly Action<object, object> ErrorDelegate;
        private static readonly Action<object, object, Exception> ErrorExceptionDelegate;
        private static readonly Action<object, string, object[]> ErrorFormatDelegate;

        private static readonly Action<object, object> FatalDelegate;
        private static readonly Action<object, object, Exception> FatalExceptionDelegate;

        private static readonly Action<object, object> DebugDelegate;
        private static readonly Action<object, object, Exception> DebugExceptionDelegate;
        private static readonly Action<object, string, object[]> DebugFormatDelegate;

        private static readonly Action<object, object> InfoDelegate;
        private static readonly Action<object, object, Exception> InfoExceptionDelegate;
        private static readonly Action<object, string, object[]> InfoFormatDelegate;

        private static readonly Action<object, object> WarnDelegate;
        private static readonly Action<object, object, Exception> WarnExceptionDelegate;
        private static readonly Action<object, string, object[]> WarnFormatDelegate;

        private readonly object logger;

        static Log4NetLogger() {
            IsErrorEnabledDelegate = GetPropertyGetter("IsErrorEnabled");
            IsFatalEnabledDelegate = GetPropertyGetter("IsFatalEnabled");
            IsDebugEnabledDelegate = GetPropertyGetter("IsDebugEnabled");
            IsInfoEnabledDelegate = GetPropertyGetter("IsInfoEnabled");
            IsWarnEnabledDelegate = GetPropertyGetter("IsWarnEnabled");
            ErrorDelegate = GetMethodCallForMessage("Error");
            ErrorExceptionDelegate = GetMethodCallForMessageException("Error");
            ErrorFormatDelegate = GetMethodCallForMessageFormat("ErrorFormat");

            FatalDelegate = GetMethodCallForMessage("Fatal");
            FatalExceptionDelegate = GetMethodCallForMessageException("Fatal");

            DebugDelegate = GetMethodCallForMessage("Debug");
            DebugExceptionDelegate = GetMethodCallForMessageException("Debug");
            DebugFormatDelegate = GetMethodCallForMessageFormat("DebugFormat");

            InfoDelegate = GetMethodCallForMessage("Info");
            InfoExceptionDelegate = GetMethodCallForMessageException("Info");
            InfoFormatDelegate = GetMethodCallForMessageFormat("InfoFormat");

            WarnDelegate = GetMethodCallForMessage("Warn");
            WarnExceptionDelegate = GetMethodCallForMessageException("Warn");
            WarnFormatDelegate = GetMethodCallForMessageFormat("WarnFormat");
        }

        private static Func<object, bool> GetPropertyGetter(string propertyName) {
            ParameterExpression funcParam = Expression.Parameter(typeof(object), "l");
            Expression convertedParam = Expression.Convert(funcParam, ILogType);
            Expression property = Expression.Property(convertedParam, propertyName);
            return (Func<object, bool>)Expression.Lambda(property, funcParam).Compile();
        }

        private static Action<object, object> GetMethodCallForMessage(string methodName) {
            ParameterExpression loggerParam = Expression.Parameter(typeof(object), "l");
            ParameterExpression messageParam = Expression.Parameter(typeof(object), "o");
            Expression convertedParam = Expression.Convert(loggerParam, ILogType);
            MethodCallExpression methodCall = Expression.Call(convertedParam, ILogType.GetMethod(methodName, new[] { typeof(object) }), messageParam);
            return (Action<object, object>)Expression.Lambda(methodCall, new[] { loggerParam, messageParam }).Compile();
        }

        private static Action<object, object, Exception> GetMethodCallForMessageException(string methodName) {
            ParameterExpression loggerParam = Expression.Parameter(typeof(object), "l");
            ParameterExpression messageParam = Expression.Parameter(typeof(object), "o");
            ParameterExpression exceptionParam = Expression.Parameter(typeof(Exception), "e");
            Expression convertedParam = Expression.Convert(loggerParam, ILogType);
            MethodCallExpression methodCall = Expression.Call(convertedParam, ILogType.GetMethod(methodName, new[] { typeof(object), typeof(Exception) }), messageParam, exceptionParam);
            return (Action<object, object, Exception>)Expression.Lambda(methodCall, new[] { loggerParam, messageParam, exceptionParam }).Compile();
        }

        private static Action<object, string, object[]> GetMethodCallForMessageFormat(string methodName) {
            ParameterExpression loggerParam = Expression.Parameter(typeof(object), "l");
            ParameterExpression formatParam = Expression.Parameter(typeof(string), "f");
            ParameterExpression parametersParam = Expression.Parameter(typeof(object[]), "p");
            Expression convertedParam = Expression.Convert(loggerParam, ILogType);
            MethodCallExpression methodCall = Expression.Call(convertedParam, ILogType.GetMethod(methodName, new[] { typeof(string), typeof(object[]) }), formatParam, parametersParam);
            return (Action<object, string, object[]>)Expression.Lambda(methodCall, new[] { loggerParam, formatParam, parametersParam }).Compile();
        }

        public Log4NetLogger(object logger) {
            this.logger = logger;
        }

        public bool IsErrorEnabled {
            get { return IsErrorEnabledDelegate(logger); }
        }

        public bool IsFatalEnabled {
            get { return IsFatalEnabledDelegate(logger); }
        }

        public bool IsDebugEnabled {
            get { return IsDebugEnabledDelegate(logger); }
        }

        public bool IsInfoEnabled {
            get { return IsInfoEnabledDelegate(logger); }
        }

        public bool IsWarnEnabled {
            get { return IsWarnEnabledDelegate(logger); }
        }

        public void Error(object message) {
            if (IsErrorEnabled)
                ErrorDelegate(logger, message);
        }

        public void Error(object message, Exception exception) {
            if (IsErrorEnabled)
                ErrorExceptionDelegate(logger, message, exception);
        }

        public void ErrorFormat(string format, params object[] args) {
            if (IsErrorEnabled)
                ErrorFormatDelegate(logger, format, args);
        }

        public void Fatal(object message) {
            if (IsFatalEnabled)
                FatalDelegate(logger, message);
        }

        public void Fatal(object message, Exception exception) {
            if (IsFatalEnabled)
                FatalExceptionDelegate(logger, message, exception);
        }

        public void Debug(object message) {
            if (IsDebugEnabled)
                DebugDelegate(logger, message);
        }

        public void Debug(object message, Exception exception) {
            if (IsDebugEnabled)
                DebugExceptionDelegate(logger, message, exception);
        }

        public void DebugFormat(string format, params object[] args) {
            if (IsDebugEnabled)
                DebugFormatDelegate(logger, format, args);
        }

        public void Info(object message) {
            if (IsInfoEnabled)
                InfoDelegate(logger, message);
        }

        public void Info(object message, Exception exception) {
            if (IsInfoEnabled)
                InfoExceptionDelegate(logger, message, exception);
        }

        public void InfoFormat(string format, params object[] args) {
            if (IsInfoEnabled)
                InfoFormatDelegate(logger, format, args);
        }

        public void Warn(object message) {
            if (IsWarnEnabled)
                WarnDelegate(logger, message);
        }

        public void Warn(object message, Exception exception) {
            if (IsWarnEnabled)
                WarnExceptionDelegate(logger, message, exception);
        }

        public void WarnFormat(string format, params object[] args) {
            if (IsWarnEnabled)
                WarnFormatDelegate(logger, format, args);
        }
    }
}