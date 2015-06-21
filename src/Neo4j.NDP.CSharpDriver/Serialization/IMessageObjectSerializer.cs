
namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Serializes <see cref="IMessageObject"/> trees into bytes in the PackStream format.
    /// </summary>
    public interface IMessageObjectSerializer
    {
        /// <summary>
        /// Serializes the <paramref name="messageObject"/> into bytes in the PackStream format.
        /// </summary>
        /// <param name="messageObject">The message object to serialize.</param>
        /// <returns>The serialized byte in the PackStream format.</returns>
        byte[] Serialize(IMessageObject messageObject);
    }
}
