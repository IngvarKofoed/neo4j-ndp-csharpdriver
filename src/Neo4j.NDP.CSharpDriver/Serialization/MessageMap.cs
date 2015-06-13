using System.Collections.Generic;
using System.Text;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a map in a PackStream message.
    /// </summary>
    /// <remarks>This is an immutable class.</remarks>
    public class MessageMap : IMessageMap
    {
        /// <summary>
        /// Instantiates an instance of <see cref="MessageMap"/> without any maps.
        /// </summary>
        public MessageMap()
        {
            this.Map = new Dictionary<IMessageObject, IMessageObject>();
        }

        /// <summary>
        /// Instantiates an instance of <see cref="MessageMap"/> with the given <see cref="map"/> map.
        /// </summary>
        /// <param name="map">The map of this map message objects.</param>
        public MessageMap(IDictionary<IMessageObject, IMessageObject> map)
        {
            this.Map = (IReadOnlyDictionary<IMessageObject, IMessageObject>)map;
        }

        /// <summary>
        /// This has the type <see cref="MessageObjectType.Map"/>
        /// </summary>
        public MessageObjectType Type { get { return MessageObjectType.Map; } }

        /// <summary>
        /// The maps of the message map objects.
        /// </summary>
        public IReadOnlyDictionary<IMessageObject, IMessageObject> Map { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("( ");
            bool isFirst = true;
            foreach (KeyValuePair<IMessageObject, IMessageObject> keyValue in Map)
            {
                if (!isFirst)
                    sb.Append(", ");
                else
                    isFirst = false;
                sb.Append(keyValue.Key.ToString());
                sb.Append("->");
                sb.Append(keyValue.Value.ToString());
            }
            sb.Append(" )");
            return sb.ToString();
        }
    }
}
