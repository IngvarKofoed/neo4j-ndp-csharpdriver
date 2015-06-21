using System;
using System.IO;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public static class MessageObjectDeserializerExtensions
    {
        /// <summary>
        /// Deserializes the given <paramref name="data"/> in the PackStream format to a <see cref="IMessageObject"/> tree.
        /// </summary>
        /// <param name="deserializer">The <see cref="IMessageObjectDeserializer"/> to use to deserialize.</param>
        /// <param name="data">The bytes to deserialize.</param>
        public static IMessageObject Deserialize(this IMessageObjectDeserializer deserializer, byte[] data)
        {
            if (deserializer == null) throw new ArgumentNullException("deserializer");

            using (MemoryStream stream = new MemoryStream(data))
            {
                return deserializer.Deserialize(stream);
            }
        }
    }
}
