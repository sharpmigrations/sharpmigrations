using System.IO;
using log4net.Config;
using Sharp.Data.Log;

namespace Sharp.Integration.Log4net {
	public class Log4NetLoggerFactory : ILoggerFactory {
	    public Log4NetLoggerFactory() {
	        XmlConfigurator.Configure();
	    }

        public Log4NetLoggerFactory(string log4NetConfigurationFile) {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(log4NetConfigurationFile));
        }

	    public ILogger GetLogger(string name) {
			return new Log4NetLogger(name);
		}
	}
}