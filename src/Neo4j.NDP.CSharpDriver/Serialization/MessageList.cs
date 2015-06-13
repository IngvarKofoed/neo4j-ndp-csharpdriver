using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a list in a PackStream message
    /// </summary>
    /// <remarks>This is an immutable class.</remarks>
    public class MessageList : IMessageList
    {
        /// <summary>
        /// Instantiates an instance of <see cref="MessageList"/> without any items.
        /// </summary>
        public MessageList()
        {
            Items = new List<IMessageObject>();
        }

        /// <summary>
        /// Instantiates a <see cref="MessageList"/> with the given items <see cref="items"/>.
        /// </summary>
        /// <param name="items">The items of the message list objects</param>
        public MessageList(IEnumerable<IMessageObject> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            Items = items.ToList();
        }

        /// <summary>
        /// This has the type <see cref="MessageObjectType.List"/>
        /// </summary>
        public MessageObjectType Type { get { return MessageObjectType.List; } }

        /// <summary>
        /// The items of this list message object. 
        /// </summary>
        public IReadOnlyList<IMessageObject> Items { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            bool isFirst = true;
            foreach (IMessageObject item in Items)
            {
                if (isFirst)
                    isFirst = false;
                else
                    sb.Append(", ");

                sb.Append(item.ToString());
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
