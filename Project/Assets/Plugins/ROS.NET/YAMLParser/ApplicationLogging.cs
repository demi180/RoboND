//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Console;

/*
namespace YAMLParser
{
    public static class ApplicationLogging
    {
        private static ILoggerFactory _loggerFactory;

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if(_loggerFactory == null)
                {
                    _loggerFactory = new LoggerFactory();
                    _loggerFactory.AddProvider(
                        new ConsoleLoggerProvider(
                            (string text, LogLevel logLevel) => { return logLevel > LogLevel.Debug;}, true)
                    );
                }
                return _loggerFactory;
            }
            set
            {
                _loggerFactory = value;
            }
        }

        public static ILogger CreateLogger<T>()
		{
			return LoggerFactory.CreateLogger<T> ();
		}

        public static ILogger CreateLogger(string category)
		{
			return LoggerFactory.CreateLogger ( category );
		}
    }

}*/