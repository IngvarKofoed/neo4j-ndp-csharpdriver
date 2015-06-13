using System;

namespace Neo4j.NDP.CSharpDriver.Logging
{
    /// <summary>
    /// Simple console logger.
    /// </summary>
    public class ConsoleLogger : IInternalLogger
    {
        public void Write(LogSeverity severity, string format, params object[] arguments)
        {
            ConsoleColor oldColor = Console.ForegroundColor;

            string prefix = "";
            if (severity == LogSeverity.Debug)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                prefix = "DEBUG";
            }
            else if (severity == LogSeverity.Information)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                prefix = "INFO ";
            }
            else if (severity == LogSeverity.Warning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                prefix = "WARN ";
            }
            else if (severity == LogSeverity.Error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                prefix = "ERROR";
            }
            else if (severity == LogSeverity.Fatal)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                prefix = "FATAL";
            }

            Console.WriteLine(prefix + ": " + string.Format(format, arguments));

            Console.ForegroundColor = oldColor;
        }
    }
}
