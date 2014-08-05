namespace Uma.DataConnector.Logging
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using NLog;

    /// <summary>
    /// Logging Wrapper for IntelliDance Application components by using NLOG 3rd party library.
    /// https://github.com/NLog/NLog
    /// http://sentinel.codeplex.com/
    /// 
    /// To create Sources in EventLog, use this command from Windows CMD (with Admin rights) on actual server:
    /// eventcreate /ID 1 /L APPLICATION /T INFORMATION  /SO "UMA eService" /D "Event Source Creating Event for eServices Web"
    /// eventcreate /ID 1 /L APPLICATION /T INFORMATION  /SO "UMA eService UmaConnWcf" /D "Event Source Creating Event for eServices UMA Conn WCF"
    /// </summary>
    /// <remarks>Regarding unit testing - its just a wrapper for NLog!</remarks>
    [ExcludeFromCodeCoverage]
    public class Log : ILog
    {
        /// <summary>
        /// Logger to handle all Logging features according to configuration
        /// </summary>
        private readonly Logger internalNLogLogger;

        /// <summary>
        /// The logger name
        /// </summary>
        private readonly string loggerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="callerName">Name of the caller.</param>
        public Log(string callerName)
        {
            if (!string.IsNullOrWhiteSpace(callerName))
            {
                this.internalNLogLogger = LogManager.GetLogger(callerName);
                this.loggerName = callerName;
                return;
            }

            this.internalNLogLogger = LogManager.GetLogger("eServices");
            this.loggerName = "eServices";
        }

        /// <summary>
        /// Writes a Tracing statement to Log appender(s). Use it for detailed debugging statements, which should be removed after code competion.
        /// Should not be found and logged enabled in Production environment.
        /// </summary>
        /// <param name="message">Any debugging statement to write to Log</param>
        public void Trace(string message)
        {
            if (this.internalNLogLogger.IsTraceEnabled)
            {
                this.internalNLogLogger.Trace(message);
            }
        }

        /// <summary>
        /// Writes a Tracing statement to Log appender(s). Use it for detailed debugging statements, which should be removed after code completion.
        /// Should not be found and logged enabled in Production environment.
        /// </summary>
        /// <param name="message">Any debugging statement to write to Log. statement is expected to contain one or more placeholders in form of {0}, {1}...</param>
        /// <param name="args">Comma separated arguments for formatted string</param>
        public void Trace(string message, params object[] args)
        {
            if (this.internalNLogLogger.IsTraceEnabled)
            {
                this.internalNLogLogger.Trace(CultureInfo.CurrentCulture, message, args);
            }
        }

        /// <summary>
        /// Writes a Tracing statement to Log appender(s). Use it for detailed debugging statements, which should be removed after code completion.
        /// Should not be found and logged enabled in Production environment.
        /// </summary>
        /// <param name="message">Any debugging statement to write to Log.</param>
        /// <param name="contextValues">
        /// The context values variable which is saved to separate field in DB or treated separately. 
        /// Purpose is to store serizalized object(s) info from running context
        /// </param>
        public void Trace(string message, string contextValues)
        {
            var info = new LogEventInfo(LogLevel.Trace, this.loggerName, message);

            if (!string.IsNullOrEmpty(contextValues))
            {
                info.Properties["ContextValues"] = contextValues;
            }

            this.internalNLogLogger.Log(info);
        }

        /// <summary>
        /// Writes a Debugging statement to Log appender(s). Use it for overall debugging statements.
        /// Should not be enabled in Production environments.
        /// </summary>
        /// <param name="message">Any debugging statement to write to Log. String is expected to contain one or more placeholders in form of {0}, {1}...</param>
        /// <param name="args">Comma separated arguments for formatted string</param>
        public void Debug(string message, params object[] args)
        {
            if (this.internalNLogLogger.IsDebugEnabled)
            {
                this.internalNLogLogger.Debug(CultureInfo.CurrentCulture, message, args);
            }
        }

        /// <summary>
        /// Writes a Debugging statement to Log appender(s). Use it for overall debugging statements.
        /// Should not be enabled in Production environments.
        /// </summary>
        /// <param name="message">Any debugging statement to write to Log</param>
        public void Debug(string message)
        {
            if (this.internalNLogLogger.IsDebugEnabled)
            {
                this.internalNLogLogger.Debug(message);
            }
        }

        /// <summary>
        /// Writes informal message to Logging appender(s). first level considered to be left for Production.
        /// May contain entry/exit point logging for application components.
        /// </summary>
        /// <param name="message">Informal message to write to Log. Message is expected to contain one or more placeholders in form of {0}, {1}...</param>
        /// <param name="args">Comma separated arguments for formatted string</param>
        public void Info(string message, params object[] args)
        {
            if (this.internalNLogLogger.IsInfoEnabled)
            {
                this.internalNLogLogger.Info(CultureInfo.CurrentCulture, message, args);
            }
        }

        /// <summary>
        /// Writes informal message to Logging appender(s). first level considered to be left for Production.
        /// May contain entry/exit point logging for application components.
        /// </summary>
        /// <param name="message">Informal message to write to Log</param>
        public void Info(string message)
        {
            if (this.internalNLogLogger.IsInfoEnabled)
            {
                this.internalNLogLogger.Info(message);
            }
        }

        /// <summary>
        /// Writes a Informational statement to Log appender(s). Use it for detailed information statements.
        /// First level of log statements in production.
        /// </summary>
        /// <param name="message">Any informal statement to write to Log.</param>
        /// <param name="contextValues">
        /// The context values variable which is saved to separate field in DB or treated separately. 
        /// Purpose is to store serizalized object(s) info from running context
        /// </param>
        public void Info(string message, string contextValues)
        {
            var info = new LogEventInfo(LogLevel.Info, this.loggerName, message);

            if (!string.IsNullOrEmpty(contextValues))
            {
                info.Properties["ContextValues"] = contextValues;
            }

            this.internalNLogLogger.Log(info);
        }

        /// <summary>
        /// Writes warning message to Logging appender(s). first level considered to be left for Production.
        /// May contain entry/exit point logging for application components.
        /// </summary>
        /// <param name="message">Warning message to write to Log. Message is expected to contain one or more placeholders in form of {0}, {1}...</param>
        /// <param name="args">Comma separated arguments for formatted string</param>
        public void Warning(string message, params object[] args)
        {
            if (this.internalNLogLogger.IsWarnEnabled)
            {
                this.internalNLogLogger.Warn(CultureInfo.CurrentCulture, message, args);
            }
        }

        /// <summary>
        /// Writes warning message to Logging appender(s). first level considered to be left for Production.
        /// May contain entry/exit point logging for application components.
        /// </summary>
        /// <param name="message">Warning message to write to Log</param>
        public void Warning(string message)
        {
            if (this.internalNLogLogger.IsWarnEnabled)
            {
                this.internalNLogLogger.Warn(message);
            }
        }

        /// <summary>
        /// To write handled error messages to logging appender(s).
        /// </summary>
        /// <param name="message">Handled error message to write to Log.</param>
        /// <param name="catchedException">Catched exception that needs to be written to Logging appeander(s)</param>
        public void Error(string message, Exception catchedException)
        {
            if (this.internalNLogLogger.IsErrorEnabled)
            {
                this.internalNLogLogger.Error(message, catchedException);
            }
        }

        /// <summary>
        /// To write handled error messages to logging appender(s).
        /// </summary>
        /// <param name="message">Handled error message to write to Log.</param>
        /// <param name="catchedException">Catched exception that needs to be written to Logging appeander(s)</param>
        /// <param name="contextValues">
        /// The context values variable which is saved to separate field in DB or treated separately. 
        /// Purpose is to store serizalized object(s) info from running context
        /// </param>
        public void Error(string message, Exception catchedException, string contextValues)
        {
            var info = new LogEventInfo(LogLevel.Error, this.loggerName, CultureInfo.CurrentCulture, message, null, catchedException);

            if (!string.IsNullOrEmpty(contextValues))
            {
                info.Properties["ContextValues"] = contextValues;
            }

            this.internalNLogLogger.Log(info);
        }

        /// <summary>
        /// To write handled error messages to logging appender(s).
        /// </summary>
        /// <param name="message">Handled error message to write to Log. Error message is expected to contain one or more placeholders in form of {0}, {1}...</param>
        /// <param name="args">Comma separated arguments for formatted string</param>
        public void Error(string message, params object[] args)
        {
            if (this.internalNLogLogger.IsErrorEnabled)
            {
                this.internalNLogLogger.Error(CultureInfo.CurrentCulture, message, args);
            }
        }

        /// <summary>
        /// To write handled error messages to logging appender(s).
        /// </summary>
        /// <param name="message">Handled error message to write to Log</param>
        public void Error(string message)
        {
            if (this.internalNLogLogger.IsErrorEnabled)
            {
                this.internalNLogLogger.Error(message);
            }
        }

        /// <summary>
        /// To write handled error messages to logging appender(s).
        /// </summary>
        /// <param name="message">Handled error message to write to Log</param>
        /// <param name="contextValues">
        /// The context values variable which is saved to separate field in DB or treated separately. 
        /// Purpose is to store serizalized object(s) info from running context
        /// </param>
        public void Error(string message, string contextValues)
        {
            var info = new LogEventInfo(LogLevel.Error, this.loggerName, message);

            if (!string.IsNullOrEmpty(contextValues))
            {
                info.Properties["ContextValues"] = contextValues;
            }

            this.internalNLogLogger.Log(info);
        }
    }
}
