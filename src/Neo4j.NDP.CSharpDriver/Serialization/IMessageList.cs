using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a list in a PackStream message
    /// </summary>
    public interface IMessageList : IMessageObject
    {
        /// <summary>
        /// The items of the message list objects.
        /// </summary>
        IReadOnlyList<IMessageObject> Items { get; }
    }
}
