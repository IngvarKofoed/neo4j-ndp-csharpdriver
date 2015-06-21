using System;
using System.Collections.Generic;
using System.IO;
using Neo4j.NDP.CSharpDriver.Extensions;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Deserializes bytes (from a stream) in the PackStream format to a <see cref="IMessageObject"/> tree.
    /// </summary>
    public class MessageObjectDeserializer : IMessageObjectDeserializer
    {
        private readonly IPackStreamUnpacker packStreamUnpacker;

       
        public MessageObjectDeserializer(IPackStreamUnpacker packStreamUnpacker)
        {
            if (packStreamUnpacker == null) throw new ArgumentNullException("packStreamUnpacker");

            this.packStreamUnpacker = packStreamUnpacker;   
        }

        /// <summary>
        /// Deserializes the given <paramref name="stream"/> by reading bytes in the PackStream format to a <see cref="IMessageObject"/> tree.
        /// </summary>
        /// <param name="stream">The stream to read the to deserialize bytes from.</param>
        public IMessageObject Deserialize(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            PackStreamUnpackerResult type = packStreamUnpacker.ReadNextType(stream);
            switch (type.Type)
            {
                case PackStreamType.Null:
                    return new MessageNull();

                case PackStreamType.Bool:
                    if (!type.BoolValue.HasValue) throw new InvalidOperationException("Expected bool value");
                    return new MessageBool(type.BoolValue.Value);

                case PackStreamType.Integer4:
                    if (!type.IntValue.HasValue) throw new InvalidOperationException("Expected length for int");
                    return new MessageInt(type.IntValue.Value);

                case PackStreamType.Integer8:
                    return DeserializeInt8(stream);

                case PackStreamType.Integer16:
                    return DeserializeInt16(stream);

                case PackStreamType.Integer32:
                    return DeserializeInt32(stream);

                case PackStreamType.Integer64:
                    return DeserializeInt64(stream);

                case PackStreamType.Text:
                    if (!type.IntValue.HasValue) throw new InvalidOperationException("Expected length for text");
                    return DeserializeText(stream, type.IntValue.Value);

                case PackStreamType.List:
                    if (!type.IntValue.HasValue) throw new InvalidOperationException("Expected length for list");
                    return DeserializeList(stream, type.IntValue.Value);

                case PackStreamType.Map:
                    if (!type.IntValue.HasValue) throw new InvalidOperationException("Expected length for map");
                    return DeserializeMap(stream, type.IntValue.Value);

                case PackStreamType.Structure:
                    if (!type.IntValue.HasValue) throw new InvalidOperationException("Expected length for structure");
                    return DeserializeStructure(stream, type.IntValue.Value);

                default:
                    throw new NotImplementedException("Unhandled pack stream type: " + type);
            }
        }

        private IMessageInt DeserializeInt8(Stream stream)
        {
            int value = packStreamUnpacker.ReadInt8(stream);
            return new MessageInt(value);
        }

        private IMessageInt DeserializeInt16(Stream stream)
        {
            int value = packStreamUnpacker.ReadInt16(stream);
            return new MessageInt(value);
        }

        private IMessageInt DeserializeInt32(Stream stream)
        {
            int value = packStreamUnpacker.ReadInt32(stream);
            return new MessageInt(value);
        }

        private IMessageInt DeserializeInt64(Stream stream)
        {
            Int64 value = packStreamUnpacker.ReadInt64(stream);
            return new MessageInt(value);
        }

        private IMessageText DeserializeText(Stream stream, int length)
        {
            if (length == 0)
            {
                return new MessageText("");
            }

            string text = packStreamUnpacker.ReadText(stream, length);

            return new MessageText(text);
        }

        private IMessageList DeserializeList(Stream stream, int itemCount)
        {
            List<IMessageObject> items = new List<IMessageObject>();
            for (int i = 0; i < itemCount; i++)
            {
                IMessageObject item = Deserialize(stream);
                items.Add(item);
            }

            return new MessageList(items);
        }

        private IMessageMap DeserializeMap(Stream stream, int mapCount)
        {
            IDictionary<IMessageObject, IMessageObject> map = new Dictionary<IMessageObject, IMessageObject>();
            for (int i = 0; i < mapCount; i++)
            {
                IMessageObject key = Deserialize(stream);
                IMessageObject value = Deserialize(stream);
                map.Add(key, value);
            }

            return new MessageMap(map);
        }

        private IMessageStructure DeserializeStructure(Stream stream, int fieldCount)
        {
            StructureSignature signature = packStreamUnpacker.ReadStructureSignature(stream);

            List<IMessageObject> fields = new List<IMessageObject>();
            for (int i = 0; i < fieldCount; i++)
            {
                IMessageObject fieldMessageObject = Deserialize(stream);
                fields.Add(fieldMessageObject);
            }

            return new MessageStructure(signature, fields);
        }
    }
}
