using System;
using System.IO;
using Neo4j.NDP.CSharpDriver.Extensions;


namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public class PackStreamUnpacker : IPackStreamUnpacker
    {
        private readonly IBitConverter bitConverter;

        public PackStreamUnpacker(IBitConverter bitConverter)
        {
            this.bitConverter = bitConverter;

        }

        public PackStreamUnpackerResult ReadNextType(Stream stream)
        {
            byte marker = (byte)stream.ReadByte();
            byte marker_low = (byte)(marker & 0x0F);
            byte marker_high = (byte)(marker & 0xF0);

            if (marker == PackStreamPacker.NullMarker)
            {
                return new PackStreamUnpackerResult(PackStreamType.Null);
            }
            else if (marker_high == PackStreamPacker.Text4BitMarker)
            {
                int length = (int)marker_low;
                return new PackStreamUnpackerResult(PackStreamType.Text, length);
            }
            if (marker == PackStreamPacker.Text8BitMarker)
            {
                int length = (int)stream.ReadByte();
                return new PackStreamUnpackerResult(PackStreamType.Text, length);
            }
            else if (marker == PackStreamPacker.Text16BitMarker)
            {
                byte[] lengthBytes = new byte[2];
                stream.Read(lengthBytes);
                int length = bitConverter.ToInt16(lengthBytes);
                return new PackStreamUnpackerResult(PackStreamType.Text, length);
            }
            else if (marker == PackStreamPacker.Text32BitMarker)
            {
                byte[] lengthBytes = new byte[4];
                stream.Read(lengthBytes);
                int length = bitConverter.ToInt32(lengthBytes);
                return new PackStreamUnpackerResult(PackStreamType.Text, length);
            }
            else if (marker_high == PackStreamPacker.List4BitMarker)
            {
                int itemCount = (int)marker_low;
                return new PackStreamUnpackerResult(PackStreamType.List, itemCount);
            }
            else if (marker_high == PackStreamPacker.Map4BitMarker)
            {
                int mapCount = (int)marker_low;
                return new PackStreamUnpackerResult(PackStreamType.Map, mapCount);
            }
            else if (marker_high == PackStreamPacker.Structure4BitMarker)
            {
                int fieldCount = (int)marker_low;
                return new PackStreamUnpackerResult(PackStreamType.Structure, fieldCount);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Marker not supported: {0:X2}", marker));
            }
        }

        public string ReadText(Stream stream, int length)
        {
            byte[] textBytes = new byte[length];
            stream.Read(textBytes);

            string text = bitConverter.ToString(textBytes);
            return text;
        }

        public StructureSignature ReadStructureSignature(Stream stream)
        {
            return (StructureSignature)stream.ReadByte();
        }
    }
}

