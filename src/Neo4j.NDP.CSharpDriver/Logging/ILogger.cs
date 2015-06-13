
namespace Neo4j.NDP.CSharpDriver.Logging
{
    /// <summary>
    /// Simple logging interface.
    /// </summary>
    /// <remarks>
    /// Marker interface. See <see cref="LogExtensions"/>
    /// </remarks>
    public interface ILogger
    {    
    }

    /// <summary>
    /// Extensions methods for handling null
    /// </summary>
    /// <example>
    /// <code>
    /// ILog log = null;
    /// log.Info("This will NOT throw an NullReferenceException");
    /// </code>
    /// </example>
    public static class LoggerExtensions
    {
        // TODO: Write documentation

        public static void Debug(this ILogger logger, string format, params object[] arguments)
        {
            IInternalLogger internalLogger = logger as IInternalLogger;
            if (internalLogger == null) return;

            internalLogger.Write(LogSeverity.Debug, format, arguments);
        }

        public static void Info(this ILogger logger, string format, params object[] arguments)
        {
            IInternalLogger internalLogger = logger as IInternalLogger;
            if (internalLogger == null) return;

            internalLogger.Write(LogSeverity.Information, format, arguments);
        }

        public static void Warn(this ILogger logger, string format, params object[] arguments)
        {
            IInternalLogger internalLogger = logger as IInternalLogger;
            if (internalLogger == null) return;

            internalLogger.Write(LogSeverity.Warning, format, arguments);
        }

        public static void Error(this ILogger logger, string format, params object[] arguments)
        {
            IInternalLogger internalLogger = logger as IInternalLogger;
            if (internalLogger == null) return;

            internalLogger.Write(LogSeverity.Error, format, arguments);
        }

        public static void Fatal(this ILogger logger, string format, params object[] arguments)
        {
            IInternalLogger internalLogger = logger as IInternalLogger;
            if (internalLogger == null) return;

            internalLogger.Write(LogSeverity.Fatal, format, arguments);
        }
    }
}
