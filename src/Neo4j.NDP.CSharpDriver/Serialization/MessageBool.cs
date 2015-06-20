using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a text in a PackStream message.
    /// </summary>
    public class MessageBool : IMessageBool
    {
        /// <summary>
        /// Instantiates a <see cref="MessageBool"/> with the bool value of <paramref name="value"/>.
        /// </summary>
        /// <param name="text">The bool value of the message bool object.</param>
        public MessageBool(bool value)
        {
            if (value == null) throw new ArgumentNullException("value");

            this.Value = value;
        }

        /// <summary>
        /// This has the type <see cref="MessageObjectType.Text"/>
        /// </summary>
        public MessageObjectType Type { get { return MessageObjectType.Bool; } }

        /// <summary>
        /// The bool  value of this bool message object.  
        /// </summary>
        public bool Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
