using System;
using System.Collections.Generic;
using System.IO;
using Neo4j.NDP.CSharpDriver.Extensions;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Deserializes bytes (from a stream) in the PackStream format to a <see cref="IMessageObject"/> tree.
    /// </summary>
    public class MessageObjectDeserializer
    {
        private readonly IPackStreamUnpacker packStreamUnpacker;

       
        public MessageObjectDeserializer(IPackStreamUnpacker packStreamUnpacker)
        {
            if (packStreamUnpacker == null) throw new ArgumentNullException("packStreamUnpacker");

            this.packStreamUnpacker = packStreamUnpacker;   
        }

        /// <summary>
        /// Deserializes the given <paramref name="data"/> bytes in the PackStream format to a <see cref="IMessageObject"/> tree.
        /// </summary>
        /// <param name="data">The bytes to deserialize from.</param>
        public IMessageObject Deserialize(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                return Deserialize(stream);
            }
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
                case PackStreamType.Text:
                    if (!type.Length.HasValue) throw new InvalidOperationException("Expected length for text");
                    return DeserializeText(stream, type.Length.Value);

                case PackStreamType.List:
                    if (!type.Length.HasValue) throw new InvalidOperationException("Expected length for list");
                    return DeserializeList(stream, type.Length.Value);

                case PackStreamType.Map:
                    if (!type.Length.HasValue) throw new InvalidOperationException("Expected length for map");
                    return DeserializeMap(stream, type.Length.Value);

                case PackStreamType.Structure:
                    if (!type.Length.HasValue) throw new InvalidOperationException("Expected length for structure");
                    return DeserializeStructure(stream, type.Length.Value);

                default:
                    throw new NotImplementedException("Unhandled pack stream type: " + type);
            }
        }

        private IMessageNull DeserializeNull()
        {
            return new MessageNull();
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

    /*
    /// <summary>
    /// Deserializes <see cref="IMessageObject"/> trees from the bytes in the given stream bytes,
    /// where the bytes are in the PackStream format.
    /// </summary>
    public class MessageObjectDeserializer
    {
        // TODO: This should be injected
        private readonly IBitConverter bitConverter = new BigEndianTargetBitConverter();

        /// <summary>
        /// Deserializes one message object in case of list, map or structure, 
        /// the children of thise message objects are also deserialized.
        /// </summary>
        /// <param name="data">An byte array to deserialize from.</param>
        /// <returns>A deserialized <see cref="IMessageObject"/>.</returns>
        public IMessageObject Deserialize(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                return Deserialize(stream);
            }
        }

        /// <summary>
        /// Deserializes one message object in case of list, map or structure, 
        /// the children of thise message objects are also deserialized.
        /// </summary>
        /// <param name="stream">The stream to read the bytes from.</param>
        /// <returns>A deserialized <see cref="IMessageObject"/>.</returns>
        public IMessageObject Deserialize(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            byte marker = (byte)stream.ReadByte();
            byte marker_low = (byte)(marker & 0x0F);
            byte marker_high = (byte)(marker & 0xF0);

            if (marker == PackStreamBuilder.NullMarker)
            {
                return DeserializeNull(stream);
            }
            else if (marker_high == PackStreamBuilder.Text4BitMarker)
            {
                int length = (int)marker_low;
                return DeserializeText(length, stream);
            }
            if (marker == PackStreamBuilder.Text8BitMarker)
            {
                int length = (int)stream.ReadByte();
                return DeserializeText(length, stream);
            }
            else if (marker == PackStreamBuilder.Text16BitMarker)
            {
                byte[] lengthBytes = new byte[2];
                stream.Read(lengthBytes);
                int length = bitConverter.ToInt16(lengthBytes);
                return DeserializeText(length, stream);
            }
            else if (marker == PackStreamBuilder.Text32BitMarker)
            {
                byte[] lengthBytes = new byte[4];
                stream.Read(lengthBytes);
                int length = bitConverter.ToInt32(lengthBytes);
                return DeserializeText(length, stream);
            }
            else if (marker_high == PackStreamBuilder.List4BitMarker)
            {
                int itemCount = (int)marker_low;
                return DeserializeList(itemCount, stream);
            }
            else if (marker_high == PackStreamBuilder.Map4BitMarker)
            {
                int mapCount = (int)marker_low;
                return DeserializeMap(mapCount, stream);
            }
            else if (marker_high == PackStreamBuilder.Structure4BitMarker)
            {
                int fieldCount = (int)marker_low;
                return DeserializeStructure(fieldCount, stream);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Marker not supported: {0:X2}", marker));
            }
        }

        private IMessageNull DeserializeNull(Stream stream)
        {
            return new MessageNull();
        }

        private IMessageText DeserializeText(int lenght, Stream stream)
        {
            if (lenght == 0)
            {
                return new MessageText("");
            }

            byte[] textBytes = new byte[lenght];
            stream.Read(textBytes);

            string text = bitConverter.ToString(textBytes);
            return new MessageText(text);
        }

        private IMessageList DeserializeList(int itemCount, Stream stream)
        {
            List<IMessageObject> items = new List<IMessageObject>();
            for (int i = 0; i < itemCount; i++)
            {
                IMessageObject item = Deserialize(stream);
                items.Add(item);
            }

            return new MessageList(items);
        }

        private IMessageMap DeserializeMap(int mapCount, Stream stream)
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

        private IMessageStructure DeserializeStructure(int fieldCount, Stream stream)
        {
            StructureSignature signature = (StructureSignature)stream.ReadByte();

            List<IMessageObject> fields = new List<IMessageObject>();
            for (int i = 0; i < fieldCount; i++)
            {
                IMessageObject fieldMessageObject = Deserialize(stream);
                fields.Add(fieldMessageObject);
            }

            return new MessageStructure(signature, fields);
        }
    }*/
}
