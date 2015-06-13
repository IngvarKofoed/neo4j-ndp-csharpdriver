
namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a null in a PackStream message.
    /// </summary>
    public class MessageNull : IMessageNull
    {
        /// <summary>
        /// This has the type <see cref="MessageObjectType.Null"/>
        /// </summary>
        public MessageObjectType Type { get { return MessageObjectType.Null; } }

        public override string ToString()
        {
            return "null";
        }
    }
}
