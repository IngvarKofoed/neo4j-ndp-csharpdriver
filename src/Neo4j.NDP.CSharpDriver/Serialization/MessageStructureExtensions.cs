using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public static class MessageStructureExtensions
    {
        /// <summary>
        /// Gets the field with the index <paramref name="fieldNumber"/> and type <typeparamref name="T"/>.
        /// If the structure does not have enough fields or the field at index <paramref name="fieldNumber"/>
        /// is not of type <typeparamref name="T"/> then null is returned.
        /// </summary>
        /// <typeparam name="T">The expected type of the field.</typeparam>
        /// <param name="messageStructure">The message structure to get the field value from.</param>
        /// <param name="fieldNumber">The index of the field value to get.</param>
        /// <returns>The found field value or null.</returns>
        public static T TryGetField<T>(this IMessageStructure messageStructure, int fieldNumber)
            where T : class, IMessageObject
        {
            if (messageStructure == null)
                throw new ArgumentNullException("messageStructure");

            if (messageStructure.Fields.Count < fieldNumber)
                return default(T);
            IMessageObject field = messageStructure.Fields[fieldNumber];
            return field as T;
        }
    }
}
