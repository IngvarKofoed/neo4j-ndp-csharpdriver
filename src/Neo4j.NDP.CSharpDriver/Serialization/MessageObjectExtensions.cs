using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public static class MessageObjectExtensions
    {
        /// <summary>
        /// Returns true if the given message object is of type <see cref="IMessageStructure"/> and has
        /// the given signature <paramref name="signature"/>. 
        /// </summary>
        /// <returns>
        /// Returns true if the given message object is of type <see cref="IMessageStructure"/> and has
        /// the given signature <paramref name="signature"/>. Otherwice false.
        /// </returns>
        /// <param name="messageObject">Message object to check for success.</param>
        /// <param name="signature">The expected signature of the message structure.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="messageObject"/> is null.</exception>
        public static bool IsStructureWithSignature(this IMessageObject messageObject, StructureSignature signature)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");

            IMessageStructure messageStructure = messageObject as IMessageStructure;
            if (messageStructure == null) return false;

            return messageStructure.Signature == signature;
        }
    }
}

