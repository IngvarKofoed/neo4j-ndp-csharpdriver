using System;
using System.IO;
using Neo4j.NDP.CSharpDriver.Extensions;


namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Reads higher level types from a PackStream.
    /// </summary>
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

        /// <summary>
        /// Reads the next type (one byte) from the given <paramref name="stream"/>.
        /// </summary>
        /// <returns>Returns the next type and in some cases (bools, null and tiny) the value is also returned.</returns>
        /// <param name="stream">The stream to read the next type from.</param>
        public PackStreamUnpackerResult ReadNextType(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            byte marker = (byte)stream.ReadByte();
            byte marker_low = (byte)(marker & 0x0F);
            byte marker_high = (byte)(marker & 0xF0);

            if (marker == PackStreamConstants.NullMarker)
            {
                return new PackStreamUnpackerResult(PackStreamType.Null);
            }
            else if (marker == PackStreamConstants.FalseMarker)
            {
                return new PackStreamUnpackerResult(PackStreamType.Bool, false);
            }
            else if (marker == PackStreamConstants.TrueMarker)
            {
                return new PackStreamUnpackerResult(PackStreamType.Bool, true);
            }
            else if (marker == PackStreamConstants.FloatMarker)
            {
                return new PackStreamUnpackerResult(PackStreamType.Double);
            }
            else if (PackStreamConstants.Int4MinByte <= marker)
            {
                // Note that Int4Min is calculated like this 256 + PackStreamConstants.Int4Min which is a large byte value
                return new PackStreamUnpackerResult(PackStreamType.Integer4, (int)marker - 256);
            }
            else if (marker <= PackStreamConstants.Int4MaxByte)
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
            else if (marker_high == PackStreamConstants.Text4Marker)
            {
                int length = (int)marker_low;
                return new PackStreamUnpackerResult(PackStreamType.Text, length);
            }
            if (marker == PackStreamConstants.Text8Marker)
            {
                int length = (int)stream.ReadByte();
                return new PackStreamUnpackerResult(PackStreamType.Text, length);
            }
            else if (marker == PackStreamConstants.Text16Marker)
            {
                byte[] lengthBytes = new byte[2];
                stream.Read(lengthBytes);
                int length = bitConverter.ToInt16(lengthBytes);
                return new PackStreamUnpackerResult(PackStreamType.Text, length);
            }
            else if (marker == PackStreamConstants.Text32Marker)
            {
                byte[] lengthBytes = new byte[4];
                stream.Read(lengthBytes);
                int length = bitConverter.ToInt32(lengthBytes);
                return new PackStreamUnpackerResult(PackStreamType.Text, length);
            }
            else if (marker_high == PackStreamConstants.List4Marker)
            {
                int length = (int)marker_low;
                return new PackStreamUnpackerResult(PackStreamType.List, length);
            }
            else if (marker == PackStreamConstants.List8Marker)
            {
                int length = stream.ReadByte();
                return new PackStreamUnpackerResult(PackStreamType.List, length);
            }
            else if (marker == PackStreamConstants.List16Marker)
            {
                byte[] lengthBytes = new byte[2];
                stream.Read(lengthBytes);
                int length = bitConverter.ToInt16(lengthBytes);
                return new PackStreamUnpackerResult(PackStreamType.List, length);
            }
            else if (marker == PackStreamConstants.List32Marker)
            {
                byte[] lengthBytes = new byte[4];
                stream.Read(lengthBytes);
                int length = bitConverter.ToInt32(lengthBytes);
                return new PackStreamUnpackerResult(PackStreamType.List, length);
            }
            else if (marker_high == PackStreamConstants.Map4Marker)
            {
                int mapCount = (int)marker_low;
                return new PackStreamUnpackerResult(PackStreamType.Map, mapCount);
            }
            else if (marker == PackStreamConstants.Map8Marker)
            {
                int length = stream.ReadByte();
                return new PackStreamUnpackerResult(PackStreamType.Map, length);
            }
            else if (marker == PackStreamConstants.Map16Marker)
            {
                byte[] lengthBytes = new byte[2];
                stream.Read(lengthBytes);
                int length = bitConverter.ToInt16(lengthBytes);
                return new PackStreamUnpackerResult(PackStreamType.Map, length);
            }
            else if (marker == PackStreamConstants.Map32Marker)
            {
                byte[] lengthBytes = new byte[4];
                stream.Read(lengthBytes);
                int length = bitConverter.ToInt32(lengthBytes);
                return new PackStreamUnpackerResult(PackStreamType.Map, length);
            }
            else if (marker_high == PackStreamConstants.Structure4Marker)
            {
                int fieldCount = (int)marker_low;
                return new PackStreamUnpackerResult(PackStreamType.Structure, fieldCount);
            }
            else if (marker == PackStreamConstants.Structure8Marker)
            {
                int length = stream.ReadByte();
                return new PackStreamUnpackerResult(PackStreamType.Structure, length);
            }
            else if (marker == PackStreamConstants.Structure16Marker)
            {
                byte[] lengthBytes = new byte[2];
                stream.Read(lengthBytes);
                int length = bitConverter.ToInt16(lengthBytes);
                return new PackStreamUnpackerResult(PackStreamType.Structure, length);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Marker not supported: {0:X2}", marker));
            }
        }

        /// <summary>
        /// Reads the next 8 bit signed integer from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="steam">The stream to read the 8 bit signed integer from.</param>
        /// <returns>The read 8 bit signed integer.</returns>
        public int ReadInt8(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            int value = stream.ReadByte();
            return value - 256;
        }

        /// <summary>
        /// Reads the next 16 bit signed integer from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="steam">The stream to read the 16 bit signed integer from.</param>
        /// <returns>The read 16 bit signed integer.</returns>
        public int ReadInt16(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            byte[] data = new byte[2];
            stream.Read(data);
            return bitConverter.ToInt16(data);
        }

        /// <summary>
        /// Reads the next 32 bit signed integer from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="steam">The stream to read the 32 bit signed integer from.</param>
        /// <returns>The read 32 bit signed integer.</returns>
        public int ReadInt32(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            byte[] data = new byte[4];
            stream.Read(data);
            return bitConverter.ToInt32(data);
        }

        /// <summary>
        /// Reads the next 64 bit signed integer from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="steam">The stream to read the 64 bit signed integer from.</param>
        /// <returns>The read 64 bit signed integer.</returns>
        public Int64 ReadInt64(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            byte[] data = new byte[8];
            stream.Read(data);
            return bitConverter.ToInt64(data);
        }

        /// <summary>
        /// Reads the next double from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="steam">The stream to read the double from.</param>
        /// <returns>The read double.</returns>
        public double ReadDouble(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            byte[] data = new byte[8];
            stream.Read(data);
            return bitConverter.ToDouble(data);
        }

        /// <summary>
        /// Reads a text with the given <paramref name="length"/> from the given <paramref name="stream"/>.
        /// </summary>
        /// <returns>Returns the read text.</returns>
        /// <param name="stream">The stream to read the text from.</param>
        /// <param name="length">The length of the text to read.</param>
        public string ReadText(Stream stream, int length)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            byte[] textBytes = new byte[length];
            stream.Read(textBytes);

            string text = bitConverter.ToString(textBytes);
            return text;
        }

        /// <summary>
        /// Reads the structure signature from the given <paramref name="stream"/>.
        /// </summary>
        /// <returns>Returns the read structure signature.</returns>
        /// <param name="stream">The stream to read the structure signature from.</param>
        public StructureSignature ReadStructureSignature(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            return (StructureSignature)stream.ReadByte();
        }
    }
}

