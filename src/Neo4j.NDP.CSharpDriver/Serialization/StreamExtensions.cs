using System;
using System.IO;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Writes a sequence of bytes with the length and given by <paramref name="data"/>
        /// byte array.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="data">The byte array to write.</param>
        public static void Write(this Stream stream, byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Reads a sequence of bytes with the length of <paramref name="data"/>
        /// byte array.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="data">The byte array to read into.</param>
        public static void Read(this Stream stream, byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
            stream.Read(data, 0, data.Length);
        }
    }
}
