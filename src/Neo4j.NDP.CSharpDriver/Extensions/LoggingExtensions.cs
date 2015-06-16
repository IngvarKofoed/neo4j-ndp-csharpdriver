using System;
using System.Collections.Generic;
using System.Text;

namespace Neo4j.NDP.CSharpDriver.Extensions
{
    public static class LoggingExtensions
    {
        /// <summary>
        /// Converts an byte array to a hex readable string.
        /// </summary>
        /// <param name="bytes">The byte to convert.</param>
        /// <returns>A hex formatted string.</returns>
        public static string ToReadableString(this byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");

            string hex = BitConverter.ToString(bytes);
            // hex = hex.Replace("-","");
            return hex;
        }

        /// <summary>
        /// Converts an enumerable of ints to a readable string.
        /// </summary>
        /// <param name="values">The ints to convert.</param>
        /// <returns>A string with all the ints.</returns>
        public static string ToReadableString(this IEnumerable<int> values)
        {
            if (values == null) throw new ArgumentNullException("values");

            StringBuilder sb = new StringBuilder();
            foreach (int value in values)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(value);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts an enumerable of strings to a readable string.
        /// </summary>
        /// <returns>A string with all the strings.</returns>
        /// <param name="values">The strings to convert.</param>
        public static string ToReadableString(this IEnumerable<string> values)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            bool isFirstLabel = true;
            foreach (string value in values)
            {
                if (isFirstLabel) isFirstLabel = false;
                else sb.Append(", ");
                sb.Append(value);
            }
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// Converts a dictionary of string->object to a readable string.
        /// </summary>
        /// <returns>A string with all the string->object values.</returns>
        /// <param name="values">The string->object values to convert.</param>
        public static string ToReadableString(this IReadOnlyDictionary<string, object> values)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            bool isFirstLabel = true;
            foreach (var value in values)
            {
                if (isFirstLabel) isFirstLabel = false;
                else sb.Append(", ");
                sb.Append(value.Key);
                sb.Append(":");
                sb.Append(value.Value);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
