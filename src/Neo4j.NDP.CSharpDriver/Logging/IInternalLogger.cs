
namespace Neo4j.NDP.CSharpDriver.Logging
{
    public enum LogSeverity
    {
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    }

    /// <summary>
    /// This is the actual logging interface that loggers should implement. <see cref="ILogger"/>.
    /// </summary>
    public interface IInternalLogger : ILogger
    {
        void Write(LogSeverity severity, string format, params object[] arguments);
    }
}
