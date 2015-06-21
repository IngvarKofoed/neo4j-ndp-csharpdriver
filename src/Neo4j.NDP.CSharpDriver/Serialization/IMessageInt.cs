using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a int value in a PackStream message.
    /// </summary>
    public interface IMessageInt : IMessageObject
    {
        /// <summary>
        /// The bool  value of this int message object. 
        /// </summary>
        Int64 Value { get; }
    }
}
