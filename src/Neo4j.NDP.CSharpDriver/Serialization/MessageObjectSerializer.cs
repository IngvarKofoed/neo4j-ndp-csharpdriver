using System;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Serializes <see cref="IMessageObject"/> trees into bytes in the PackStream format.
    /// </summary>
    public class MessageObjectSerializer
    {
        private readonly IPackSteamBuilderFactory packSteamBuilderFactory;

        public MessageObjectSerializer(IPackSteamBuilderFactory packSteamBuilderFactory)
        {
            if (packSteamBuilderFactory == null) throw new ArgumentNullException("packSteamBuilderFactory");

            this.packSteamBuilderFactory = packSteamBuilderFactory;
        }

        /// <summary>
        /// Serializes the <paramref name="messageObject"/> into bytes in the PackStream format.
        /// </summary>
        /// <param name="messageObject">The message object to serialize.</param>
        /// <returns>The serialized byte in the PackStream format.</returns>
        public byte[] Serialize(IMessageObject messageObject)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");

            IPackStreamBuilder builder = packSteamBuilderFactory.Create();
            Serialize(messageObject, builder);
            return builder.GetBytes();
        }

        private void Serialize(IMessageObject messageObject, IPackStreamBuilder builder)
        {
            switch (messageObject.Type)
            {
                case MessageObjectType.Text:
                    Serialize((IMessageText)messageObject, builder);
                    break;

                case MessageObjectType.List:
                    Serialize((IMessageList)messageObject, builder);
                    break;

                case MessageObjectType.Map:
                    Serialize((IMessageMap)messageObject, builder);
                    break;

                case MessageObjectType.Structure:
                    Serialize((IMessageStructure)messageObject, builder);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void Serialize(IMessageText messageText, IPackStreamBuilder builder)
        {
            builder.Append(messageText.Text);
        }

        private void Serialize(IMessageList messageList, IPackStreamBuilder builder)
        {
            builder.AppendListHeader(messageList.Items.Count);
            foreach (IMessageObject item in messageList.Items)
            {
                Serialize(item, builder);
            }
        }

        private void Serialize(IMessageMap messageMap, IPackStreamBuilder builder)
        {
            builder.AppendMapHeader(messageMap.Map.Count);
            foreach (KeyValuePair<IMessageObject, IMessageObject> pair in messageMap.Map)
            {
                Serialize(pair.Key, builder);
                Serialize(pair.Value, builder);
            }
        }

        private void Serialize(IMessageStructure messageStructure, IPackStreamBuilder builder)
        {
            builder.AppendStructureHeader(messageStructure.Signature, messageStructure.Fields.Count);
            foreach (IMessageObject fieldMessageObject in messageStructure.Fields)
            {
                Serialize(fieldMessageObject, builder);
            }
        }
    }
}
