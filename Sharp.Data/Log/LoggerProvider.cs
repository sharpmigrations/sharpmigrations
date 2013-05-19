using System;
using System.IO;

namespace Sharp.Data.Log {
    public class LogManager {
        private ISharpLoggerFactory _loggerFactory;
        private static LogManager _instance;

        static LogManager() {
            string loggerClass = TryToFindLog4Net();
            ISharpLoggerFactory loggerFactory = String.IsNullOrEmpty(loggerClass) ? new NoLoggingLoggerFactory() : GetLoggerFactory(loggerClass);
            SetLoggersFactory(loggerFactory);
        }

        private static ISharpLoggerFactory GetLoggerFactory(string loggerClass) {
            ISharpLoggerFactory loggerFactory;
            var loggerFactoryType = Type.GetType(loggerClass);
            try {
                loggerFactory = (ISharpLoggerFactory)Activator.CreateInstance(loggerFactoryType);
            }
            catch (MissingMethodException ex) {
                throw new ApplicationException("Public constructor was not found for " + loggerFactoryType, ex);
            }
            catch (InvalidCastException ex) {
                throw new ApplicationException(loggerFactoryType + "Type does not implement " + typeof(ISharpLoggerFactory), ex);
            }
            catch (Exception ex) {
                throw new ApplicationException("Unable to instantiate: " + loggerFactoryType, ex);
            }
            return loggerFactory;
        }

        private static string TryToFindLog4Net() {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
            string binPath = relativeSearchPath == null ? baseDir : Path.Combine(baseDir, relativeSearchPath);
            var log4NetDllPath = Path.Combine(binPath, "log4net.dll");
            if (File.Exists(log4NetDllPath)) {
                return typeof(Log4NetLoggerFactory).AssemblyQualifiedName;
            }
            return null;
        }

        public static void SetLoggersFactory(ISharpLoggerFactory loggerFactory) {
            _instance = new LogManager(loggerFactory);
        }

        private LogManager(ISharpLoggerFactory loggerFactory) {
            _loggerFactory = loggerFactory;
        }

        public static ISharpLogger GetLogger(string keyName) {
            return _instance._loggerFactory.LoggerFor(keyName);
        }

        public static ISharpLogger GetLogger(Type type) {
            return _instance._loggerFactory.LoggerFor(type);
        }
    }
}