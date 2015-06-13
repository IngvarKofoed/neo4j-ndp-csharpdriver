using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a map in a PackStream message.
    /// </summary>
    public interface IMessageMap : IMessageObject
    {
        /// <summary>
        /// The maps of the message map objects.
        /// </summary>
        IReadOnlyDictionary<IMessageObject, IMessageObject> Map { get; }
    }
}
