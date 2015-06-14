using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public static class MessageObjectExtensions
    {
        /// <summary>
        /// Returns true if the given message object is a <see cref="IMessageStructure"/> and has
        /// the signature <see cref="StructureSignature.Success"/>. 
        /// </summary>
        /// <returns>True if is a message structure if success signature. Otherwice false.</returns>
        /// <param name="messageObject">Message object to check for success.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="messageObject"/> is null.</exception>
        public static bool IsSuccess(this IMessageObject messageObject)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");

            IMessageStructure messageStructure = messageObject as IMessageStructure;
            if (messageStructure == null) return false;

            return messageStructure.Signature == StructureSignature.Success;
        }

        /// <summary>
        /// Returns true if the given message object is a <see cref="IMessageStructure"/> and has
        /// the signature <see cref="StructureSignature.Failure"/>. 
        /// </summary>
        /// <returns>True if is a message structure if failure signature. Otherwice false.</returns>
        /// <param name="messageObject">Message object to check for failure.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="messageObject"/> is null.</exception>
        public static bool IsFailure(this IMessageObject messageObject)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");

            IMessageStructure messageStructure = messageObject as IMessageStructure;
            if (messageStructure == null) return false;

            return messageStructure.Signature == StructureSignature.Failure;
        }
    }
}

