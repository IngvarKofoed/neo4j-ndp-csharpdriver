using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a text in a PackStream message
    /// </summary>
    /// <remarks>This is an immutable class.</remarks>
    public class MessageText : IMessageText
    {
        /// <summary>
        /// Instantiates a <see cref="MessageText"/> with the text value of <paramref name="text"/>.
        /// </summary>
        /// <param name="text">The text value of the message text object.</param>
        public MessageText(string text)
        {
            if (text == null) throw new ArgumentNullException("text");

            this.Text = text;
        }

        /// <summary>
        /// This has the type <see cref="MessageObjectType.Text"/>
        /// </summary>
        public MessageObjectType Type { get { return MessageObjectType.Text; } }

        /// <summary>
        /// The text string value of this text message object. 
        /// </summary>
        public string Text { get; private set; }

        public override string ToString()
        {
            return string.Format("\"{0}\"", Text);
        }
    }
}
