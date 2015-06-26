using System;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// This builds an byte array in the PackStream format.
    /// It is building it like the System.Text.StringBuilder.
    /// </summary>
    public sealed class PackStreamPacker : IPackStreamPacker
    {
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
        /// Appends an null value to the result byte array.
        /// </summary>
        public void AppendNull()
        {
            bytes.Add(PackStreamConstants.NullMarker);
        }

        /// <summary>
        /// Appends the given <paramref name="value"/> bool balue to the result byte array.
        /// </summary>
        /// <param name="value">The bool value to append.</param>
        public void Append(bool value)
        {
            if (value)
            {
                bytes.Add(PackStreamConstants.TrueMarker);
            }
            else
            {
                bytes.Add(PackStreamConstants.FalseMarker);
            }
        }

        /// <summary>
        /// Appends the given <paramref name="value"/> value to the result byte array.
        /// </summary>
        /// <param name="value">The int value to append.</param>
        public void Append(double value)
        {
            bytes.Add(PackStreamConstants.FloatMarker);
            bytes.AddRange(bitConverter.GetBytes(value));
        }

        /// <summary>
        /// Appends the given <paramref name="value"/> value to the result byte array.
        /// </summary>
        /// <param name="value">The int value to append.</param>
        public void Append(Int64 value)
        {
            if (PackStreamConstants.Int4Min <= value && value <= PackStreamConstants.Int4Max)
            {
                bytes.Add((byte)value);
            }
            else if (PackStreamConstants.Int8Min <= value && value <= PackStreamConstants.Int8Max)
            {
                bytes.Add(PackStreamConstants.Int8Marker);
                bytes.Add((byte)value);
            }
            else if (PackStreamConstants.Int16Min <= value && value <= PackStreamConstants.Int16Max)
            {
                bytes.Add(PackStreamConstants.Int16Marker);
                bytes.AddRange(bitConverter.GetBytes((short)value));
            }
            else if (PackStreamConstants.Int32Min <= value && value <= PackStreamConstants.Int32Max)
            {
                bytes.Add(PackStreamConstants.Int32Marker);
                bytes.AddRange(bitConverter.GetBytes((int)value));
            }
            else if (PackStreamConstants.Int64Min <= value && value <= PackStreamConstants.Int64Max)
            {
                bytes.Add(PackStreamConstants.Int64Marker);
                bytes.AddRange(bitConverter.GetBytes(value));
            }
        }

        /// <summary>
        /// Appends the given <paramref name="text"/> UTF8 encoded, to the result byte array.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void Append(string text)
        {
            byte[] textBytes = bitConverter.GetBytes(text);

            if (textBytes.Length <= 15)
            {
                byte marker = (byte)(PackStreamConstants.Text4Marker + textBytes.Length);
                bytes.Add(marker);
            }
            else if (textBytes.Length <= 255)
            {
                bytes.Add(PackStreamConstants.Text8Marker);
                byte size = (byte)textBytes.Length;
                bytes.Add(size);
            }
            else if (textBytes.Length <= 65535)
            {
                bytes.Add(PackStreamConstants.Text16Marker);
                ushort size = (ushort)textBytes.Length;
                bytes.AddRange(bitConverter.GetBytes(size));
            }
            else if (textBytes.Length <= int.MaxValue) // Shoudl be 4294967295, but List<>.Count is returning int, not uint
            {
                bytes.Add(PackStreamConstants.Text32Marker);
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
                byte marker = (byte)(PackStreamConstants.List4Marker + listElementCount);
                bytes.Add(marker);
            }
            else if (listElementCount < 256)
            {
                bytes.Add(PackStreamConstants.List8Marker);
                bytes.AddRange(bitConverter.GetBytes((byte)listElementCount));
            }
            else if (listElementCount < 65536)
            {
                bytes.Add(PackStreamConstants.List16Marker);
                bytes.AddRange(bitConverter.GetBytes((short)listElementCount));
            }
            else if (listElementCount < int.MaxValue) // Shoudl be 4294967295, but List<>.Count is returning int, not uint
            {
                bytes.Add(PackStreamConstants.List32Marker);
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
                byte marker = (byte)(PackStreamConstants.Map4Marker + mapElementCount);
                bytes.Add(marker);
            }
            else if (mapElementCount < 256)
            {
                bytes.Add(PackStreamConstants.Map8Marker);
                bytes.AddRange(bitConverter.GetBytes((byte)mapElementCount));
            }
            else if (mapElementCount < 65536)
            {
                bytes.Add(PackStreamConstants.Map16Marker);
                bytes.AddRange(bitConverter.GetBytes((short)mapElementCount));
            }
            else if (mapElementCount < int.MaxValue) // Shoudl be 4294967295, but Dictionary<,>.Count is returning int, not uint
            {
                bytes.Add(PackStreamConstants.Map32Marker);
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
                byte marker = (byte)(PackStreamConstants.Structure4Marker + fieldInStructureCount);
                bytes.Add(marker);
                bytes.Add((byte)signature);
            }
            else if (fieldInStructureCount < 256)
            {
                bytes.Add(PackStreamConstants.Structure8Marker);
                bytes.AddRange(bitConverter.GetBytes((byte)fieldInStructureCount));
                bytes.Add((byte)signature);
            }
            else if (fieldInStructureCount < 65536)
            {
                bytes.Add(PackStreamConstants.Structure16Marker);
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
