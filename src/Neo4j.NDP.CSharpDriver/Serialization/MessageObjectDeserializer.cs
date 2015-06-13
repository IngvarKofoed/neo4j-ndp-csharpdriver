using System;
using System.Collections.Generic;
using System.IO;
using Neo4j.NDP.CSharpDriver.Extensions;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
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
    }
}
