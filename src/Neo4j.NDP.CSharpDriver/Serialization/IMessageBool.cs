

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a bool value in a PackStream message.
    /// </summary>
    public interface IMessageBool : IMessageObject
    {
        /// <summary>
        /// The bool  value of this bool message object. 
        /// </summary>
        bool Value { get; }
    }
}
