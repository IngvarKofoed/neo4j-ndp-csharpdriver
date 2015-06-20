using System;
using System.IO;
using Neo4j.NDP.CSharpDriver.Extensions;


namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public class PackStreamUnpacker : IPackStreamUnpacker
    {
        private readonly IBitConverter bitConverter;

        /// <summary>
        /// Instantiates an <see cref="PackStreamUnpacker"/> instance.
        /// </summary>
        /// <param name="bitConverter">The <see cref="IBitConverter"/> to use when converting bytes from the pack stream.</param>
        public PackStreamUnpacker(IBitConverter bitConverter)
        {
            if (bitConverter == null) throw new ArgumentNullException("bitConverter");

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
            else if (marker == PackStreamPacker.FalseMarker)
            {
                return new PackStreamUnpackerResult(PackStreamType.Bool, false);
            }
            else if (marker == PackStreamPacker.TrueMarker)
            {
                return new PackStreamUnpackerResult(PackStreamType.Bool, true);
            }
            else if (PackStreamConstants.Int4Min <= marker)
            {
                // Note that Int4Min is calculated like this 256 + PackStreamConstants.Int4Min which is a large byte value
                return new PackStreamUnpackerResult(PackStreamType.Integer4, (int)marker - 256);
            }
            else if (marker <= PackStreamConstants.Int4Max)
            {
                return new PackStreamUnpackerResult(PackStreamType.Integer4, (int)marker);
            }
            else if (marker == PackStreamConstants.Int8Marker)
            {
                return new PackStreamUnpackerResult(PackStreamType.Integer8, 1);
            }
            else if (marker == PackStreamConstants.Int16Marker)
            {
                return new PackStreamUnpackerResult(PackStreamType.Integer16, 2);
            }
            else if (marker == PackStreamConstants.Int32Marker)
            {
                return new PackStreamUnpackerResult(PackStreamType.Integer32, 4);
            }
            else if (marker == PackStreamConstants.Int64Marker)
            {
                return new PackStreamUnpackerResult(PackStreamType.Integer64, 8);
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

        public int ReadInt8(Stream stream)
        {
            int value = stream.ReadByte();
            return value - 256;
        }

        public int ReadInt16(Stream stream)
        {
            byte[] data = new byte[2];
            stream.Read(data);
            return bitConverter.ToInt16(data);
        }

        public int ReadInt32(Stream stream)
        {
            byte[] data = new byte[4];
            stream.Read(data);
            return bitConverter.ToInt32(data);
        }

        public Int64 ReadInt64(Stream stream)
        {
            byte[] data = new byte[8];
            stream.Read(data);
            return bitConverter.ToInt64(data);
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

