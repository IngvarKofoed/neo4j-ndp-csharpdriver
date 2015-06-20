using System;
using System.Collections.Generic;
using System.Text;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// This builds an byte array in the PackStream format.
    /// It is building it like the System.Text.StringBuilder.
    /// </summary>
    public sealed class PackStreamPacker : IPackStreamPacker
    {
        // Simple
        public static readonly byte NullMarker = 0xC0;
        public static readonly byte TrueMarker = 0xC3;
        public static readonly byte FalseMarker = 0xC2;
        // Floats (63bits)
        // TODO: Add markers for floats
        // Integer
        // TODO: Add markers for integers
        // Text
        public static readonly byte Text4BitMarker = 0x80;
        public static readonly byte Text8BitMarker = 0xD0;
        public static readonly byte Text16BitMarker = 0xD1;
        public static readonly byte Text32BitMarker = 0xD2;
        // List
        public static readonly byte List4BitMarker = 0x90;
        public static readonly byte List8BitMarker = 0xD4;
        public static readonly byte List16BitMarker = 0xD5;
        public static readonly byte List32BitMarker = 0xD6;
        // Map
        public static readonly byte Map4BitMarker = 0xA0;
        public static readonly byte Map8BitMarker = 0xD8;
        public static readonly byte Map16BitMarker = 0xD9;
        public static readonly byte Map32BitMarker = 0xDA;
        // Structure
        public static readonly byte Structure4BitMarker = 0xB0;
        public static readonly byte Structure8BitMarker = 0xDC;
        public static readonly byte Structure16BitMarker = 0xDD;

        private readonly IBitConverter bitConverter;
        
        private List<byte> bytes = new List<byte>();

        /// <summary>
        /// Constructs a <see cref="PackStreamPacker"/>.
        /// </summary>
        /// <param name="bitConverter">The bit converter to use.</param>
        public PackStreamPacker(IBitConverter bitConverter)
        {
            this.bitConverter = bitConverter;
        }

        /// <summary>
        /// Appends the given <paramref name="text"/> UTF8 encoded, to the reulst byte array.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void Append(string text)
        {
            // TODO: Use the bit converter here
            byte[] textBytes = bitConverter.GetBytes(text);

            if (textBytes.Length <= 15)
            {
                byte marker = (byte)(Text4BitMarker + textBytes.Length);
                bytes.Add(marker);
            }
            else if (textBytes.Length <= 255)
            {
                bytes.Add(Text8BitMarker);
                byte size = (byte)textBytes.Length;
                bytes.Add(size);
            }
            else if (textBytes.Length <= 65535)
            {
                bytes.Add(Text16BitMarker);
                ushort size = (ushort)textBytes.Length;
                bytes.AddRange(bitConverter.GetBytes(size));
            }
            else if (textBytes.Length <= int.MaxValue) // Shoudl be 4294967295, but List<>.Count is returning int, not uint
            {
                bytes.Add(Text32BitMarker);
                int size = textBytes.Length;
                bytes.AddRange(bitConverter.GetBytes(size));
            }

            bytes.AddRange(textBytes);
        }

        /// <summary>
        /// Appends a list header to the result byte array.
        /// A total of <paramref name="listElementCount"/> items are expected to be added.
        /// </summary>
        /// <param name="listElementCount">The number of expted items to be added after the header.</param>
        public void AppendListHeader(int listElementCount)
        {
            if (listElementCount < 16)
            {
                byte marker = (byte)(List4BitMarker + listElementCount);
                bytes.Add(marker);
            }
            else if (listElementCount < 256)
            {
                bytes.Add(List8BitMarker);
                bytes.AddRange(bitConverter.GetBytes((byte)listElementCount));
            }
            else if (listElementCount < 65536)
            {
                bytes.Add(List16BitMarker);
                bytes.AddRange(bitConverter.GetBytes((short)listElementCount));
            }
            else if (listElementCount < int.MaxValue) // Shoudl be 4294967295, but List<>.Count is returning int, not uint
            {
                bytes.Add(List32BitMarker);
                bytes.AddRange(bitConverter.GetBytes(listElementCount));
            }
        }

        /// <summary>
        /// Appends a map header to the result byte array.
        /// A total of <paramref name="mapElementCount"/> maps are expected to be added.
        /// </summary>
        /// <param name="listElementCount">The number of expted maps to be added after the header.</param>
        public void AppendMapHeader(int mapElementCount)
        {
            if (mapElementCount < 16)
            {
                byte marker = (byte)(Map4BitMarker + mapElementCount);
                bytes.Add(marker);
            }
            else if (mapElementCount < 256)
            {
                bytes.Add(Map8BitMarker);
                bytes.AddRange(bitConverter.GetBytes((byte)mapElementCount));
            }
            else if (mapElementCount < 65536)
            {
                bytes.Add(Map16BitMarker);
                bytes.AddRange(bitConverter.GetBytes((short)mapElementCount));
            }
            else if (mapElementCount < int.MaxValue) // Shoudl be 4294967295, but Dictionary<,>.Count is returning int, not uint
            {
                bytes.Add(Map32BitMarker);
                bytes.AddRange(bitConverter.GetBytes(mapElementCount));
            }
        }

        /// <summary>
        /// Appends a structure header to the result byte array.
        /// A total of <paramref name="mapElementCount"/> fields are expected to be added.
        /// </summary>
        /// <param name="signature">The <see cref="Signature"/> of the structure.</param>
        /// <param name="fieldInStructureCount">The number of expted fields to be added after the header.</param>
        public void AppendStructureHeader(StructureSignature signature, int fieldInStructureCount)
        {
            if (fieldInStructureCount < 16)
            {
                byte marker = (byte)(Structure4BitMarker + fieldInStructureCount);
                bytes.Add(marker);
                bytes.Add((byte)signature);
            }
            else if (fieldInStructureCount < 256)
            {
                bytes.Add(Structure8BitMarker);
                bytes.AddRange(bitConverter.GetBytes((byte)fieldInStructureCount));
                bytes.Add((byte)signature);
            }
            else if (fieldInStructureCount < 65536)
            {
                bytes.Add(Structure16BitMarker);
                bytes.AddRange(bitConverter.GetBytes((short)fieldInStructureCount));
                bytes.Add((byte)signature);
            }
            else
            {
                throw new ArgumentException("fieldInStructureCount should be less that 65536");
            }
        }

        /// <summary>
        /// Retuns the result byte array.
        /// </summary>
        /// <returns>Retuns the result byte array.</returns>
        public byte[] GetBytes()
        {
            return bytes.ToArray();
        }
    }
}
