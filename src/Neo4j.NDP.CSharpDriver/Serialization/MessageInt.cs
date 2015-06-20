using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents an int value in a PackStream message.
    /// </summary>
    public class MessageInt : IMessageInt
    {
        /// <summary>
        /// Instantiates a <see cref="MessageInt"/> with the int value of <paramref name="value"/>.
        /// </summary>
        /// <param name="text">The int value of the message int object.</param>
        public MessageInt(Int64 value)
        {
            if (value == null) throw new ArgumentNullException("value");

            this.Value = value;
        }

        /// <summary>
        /// This has the type <see cref="MessageObjectType.Text"/>
        /// </summary>
        public MessageObjectType Type { get { return MessageObjectType.Int; } }

        /// <summary>
        /// The bool  value of this bool message object.  
        /// </summary>
        public Int64 Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
