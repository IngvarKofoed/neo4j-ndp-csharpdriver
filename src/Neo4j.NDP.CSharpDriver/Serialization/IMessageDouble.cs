using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a double value in a PackStream message.
    /// </summary>
    public interface IMessageDouble : IMessageObject
    {
        /// <summary>
        /// The bool  value of this int message object. 
        /// </summary>
        double Value { get; }
    }
}
