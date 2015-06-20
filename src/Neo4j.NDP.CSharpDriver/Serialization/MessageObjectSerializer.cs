using System;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Serializes <see cref="IMessageObject"/> trees into bytes in the PackStream format.
    /// </summary>
    public class MessageObjectSerializer
    {
        private readonly IPackStreamPackerFactory packSteamPackerFactory;

        public MessageObjectSerializer(IPackStreamPackerFactory packSteamPackerFactory)
        {
            if (packSteamPackerFactory == null) throw new ArgumentNullException("packSteamBuilderFactory");

            this.packSteamPackerFactory = packSteamPackerFactory;
        }

        /// <summary>
        /// Serializes the <paramref name="messageObject"/> into bytes in the PackStream format.
        /// </summary>
        /// <param name="messageObject">The message object to serialize.</param>
        /// <returns>The serialized byte in the PackStream format.</returns>
        public byte[] Serialize(IMessageObject messageObject)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");

            IPackStreamPacker builder = packSteamPackerFactory.Create();
            Serialize(messageObject, builder);
            return builder.GetBytes();
        }

        private void Serialize(IMessageObject messageObject, IPackStreamPacker builder)
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

        private void Serialize(IMessageText messageText, IPackStreamPacker builder)
        {
            builder.Append(messageText.Text);
        }

        private void Serialize(IMessageList messageList, IPackStreamPacker builder)
        {
            builder.AppendListHeader(messageList.Items.Count);
            foreach (IMessageObject item in messageList.Items)
            {
                Serialize(item, builder);
            }
        }

        private void Serialize(IMessageMap messageMap, IPackStreamPacker builder)
        {
            builder.AppendMapHeader(messageMap.Map.Count);
            foreach (KeyValuePair<IMessageObject, IMessageObject> pair in messageMap.Map)
            {
                Serialize(pair.Key, builder);
                Serialize(pair.Value, builder);
            }
        }

        private void Serialize(IMessageStructure messageStructure, IPackStreamPacker builder)
        {
            builder.AppendStructureHeader(messageStructure.Signature, messageStructure.Fields.Count);
            foreach (IMessageObject fieldMessageObject in messageStructure.Fields)
            {
                Serialize(fieldMessageObject, builder);
            }
        }
    }
}
