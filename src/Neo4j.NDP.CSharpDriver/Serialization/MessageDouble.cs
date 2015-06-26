using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents an int value in a PackStream message.
    /// </summary>
    public class MessageDouble : IMessageDouble
    {
        /// <summary>
        /// Instantiates a <see cref="MessageInt"/> with the double value of <paramref name="value"/>.
        /// </summary>
        /// <param name="text">The double value of the message double object.</param>
        public MessageDouble(double value)
        {
            this.Value = value;
        }

        /// <summary>
        /// This has the type <see cref="MessageObjectType.Text"/>
        /// </summary>
        public MessageObjectType Type { get { return MessageObjectType.Double; } }

        /// <summary>
        /// The bool  value of this bool message object.  
        /// </summary>
        public double Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
