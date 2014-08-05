namespace Uma.Eservices.Common
{
    using System;

    /// <summary>
    /// Interface of Logging component
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Writes a Tracing statement to Log appender(s). Use it for detailed debugging statements, which should be removed after code competion.
        /// Should not be found and logged enabled in Production environment.
        /// </summary>
        /// <param name="message">Any debugging statement to write to Log</param>
        void Trace(string message);

        /// <summary>
        /// Writes a Tracing statement to Log appender(s). Use it for detailed debugging statements, which should be removed after code competion.
        /// Should not be found and logged enabled in Production environment.
        /// </summary>
        /// <param name="message">Any debugging statement to write to Log. statement is expected to contain one or more placeholders in form of {0}, {1}...</param>
        /// <param name="args">Comma separated arguments for formatted string </param>
        void Trace(string message, params object[] args);

        /// <summary>
        /// Writes a Tracing statement to Log appender(s). Use it for detailed debugging statements, which should be removed after code completion.
        /// Should not be found and logged enabled in Production environment.
        /// </summary>
        /// <param name="message">Any debugging statement to write to Log.</param>
        /// <param name="contextValues">
        /// The context values variable which is saved to separate field in DB or treated separately. 
        /// Purpose is to store serizalized object(s) info from running context
        /// </param>
        void Trace(string message, string contextValues);

        /// <summary>
        /// Writes a Debugging statement to Log appender(s). Use it for overall debugging statements.
        /// Should not be enabled in Production environments.
        /// </summary>
        /// <param name="message">Any debugging statement to write to Log. String is expected to contain one or more placeholders in form of {0}, {1}...</param>
        /// <param name="args">Comma separated arguments for formatted string </param>
        void Debug(string message, params object[] args);

        /// <summary>
        /// Writes a Debugging statement to Log appender(s). Use it for overall debugging statements.
        /// Should not be enabled in Production environments.
        /// </summary>
        /// <param name="message">Any debugging statement to write to Log</param>
        void Debug(string message);

        /// <summary>
        /// Writes informal message to Logging appender(s). first level considered to be left for Production.
        /// May contain entry/exit point logging for application components.
        /// </summary>
        /// <param name="message">Informal message to write to Log. Message is expected to contain one or more placeholders in form of {0}, {1}...</param>
        /// <param name="args">Comma separated arguments for formatted string </param>
        void Info(string message, params object[] args);

        /// <summary>
        /// Writes informal message to Logging appender(s). first level considered to be left for Production.
        /// May contain entry/exit point logging for application components.
        /// </summary>
        /// <param name="message">Informal message to write to Log</param>
        void Info(string message);

        /// <summary>
        /// Writes a Informational statement to Log appender(s). Use it for detailed information statements.
        /// First level of log statements in production.
        /// </summary>
        /// <param name="message">Any informal statement to write to Log.</param>
        /// <param name="contextValues">
        /// The context values variable which is saved to separate field in DB or treated separately. 
        /// Purpose is to store serizalized object(s) info from running context
        /// </param>
        void Info(string message, string contextValues);

        /// <summary>
        /// Writes warning message to Logging appender(s). first level considered to be left for Production.
        /// May contain entry/exit point logging for application components.
        /// </summary>
        /// <param name="message">Warning message to write to Log. Message is expected to contain one or more placeholders in form of {0}, {1}...</param>
        /// <param name="args">Comma separated arguments for formatted string </param>
        void Warning(string message, params object[] args);

        /// <summary>
        /// Writes warning message to Logging appender(s). first level considered to be left for Production.
        /// May contain entry/exit point logging for application components.
        /// </summary>
        /// <param name="message">Warning message to write to Log</param>
        void Warning(string message);

        /// <summary>
        /// To write handled error messages to logging appender(s).
        /// </summary>
        /// <param name="message">Handled error message to write to Log.</param>
        /// <param name="catchedException">Catched exception that needs to be written to Logging appeander(s)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error", Justification = "This is working solution for Logging.")]
        void Error(string message, Exception catchedException);

        /// <summary>
        /// To write handled error messages to logging appender(s).
        /// </summary>
        /// <param name="message">Handled error message to write to Log.</param>
        /// <param name="catchedException">Catched exception that needs to be written to Logging appeander(s)</param>
        /// <param name="contextValues">
        /// The context values variable which is saved to separate field in DB or treated separately. 
        /// Purpose is to store serizalized object(s) info from running context
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error", Justification = "This is working solution for Logging.")]
        void Error(string message, Exception catchedException, string contextValues);

        /// <summary>
        /// To write handled error messages to logging appender(s).
        /// </summary>
        /// <param name="message">Handled error message to write to Log. Error message is expected to contain one or more placeholders in form of {0}, {1}...</param>
        /// <param name="args">Comma separated arguments for formatted string </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error", Justification = "This is working solution for Logging.")]
        void Error(string message, params object[] args);

        /// <summary>
        /// To write handled error messages to logging appender(s).
        /// </summary>
        /// <param name="message">Handled error message to write to Log</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error", Justification = "This is working solution for Logging.")]
        void Error(string message);

        /// <summary>
        /// To write handled error messages to logging appender(s).
        /// </summary>
        /// <param name="message">Handled error message to write to Log</param>
        /// <param name="contextValues">
        /// The context values variable which is saved to separate field in DB or treated separately. 
        /// Purpose is to store serizalized object(s) info from running context
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error", Justification = "This is working solution for Logging.")]
        void Error(string message, string contextValues);
    }
}
