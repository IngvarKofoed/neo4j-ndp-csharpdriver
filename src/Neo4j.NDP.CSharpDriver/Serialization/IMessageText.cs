
namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a text in a PackStream message.
    /// </summary>
    public interface IMessageText : IMessageObject
    {
        /// <summary>
        /// The text string value of this text message object. 
        /// </summary>
        string Text { get; }
    }
}
