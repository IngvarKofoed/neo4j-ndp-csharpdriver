using System.IO;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Deserializes bytes (from a stream) in the PackStream format to a <see cref="IMessageObject"/> tree.
    /// </summary>
    public interface IMessageObjectDeserializer
    {
        /// <summary>
        /// Deserializes the given <paramref name="stream"/> by reading bytes in the PackStream format to a <see cref="IMessageObject"/> tree.
        /// </summary>
        /// <param name="stream">The stream to read the to deserialize bytes from.</param>
        IMessageObject Deserialize(Stream stream);
    }
}
