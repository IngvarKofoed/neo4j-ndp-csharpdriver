
using System;
namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public static class PackStreamConstants
    {
        // Simple
        public const byte NullMarker = 0xC0;
        public const byte TrueMarker = 0xC3;
        public const byte FalseMarker = 0xC2;

        // Floats (63bits)
        public const byte FloatMarker = 0xC1;

        // Integers markers and constants
        // Note: Int4 has no marker but has the raw value of [-16, 127] or in hex: 0xF0-0xFF and 0x00-0x7F
        public const byte Int8Marker = 0xC8;
        public const byte Int16Marker = 0xC9;
        public const byte Int32Marker = 0xCA;
        public const byte Int64Marker = 0xCB;

        public const byte Int4MinByte = (byte)(256 - 16);
        public const byte Int4MaxByte = (byte)127;
        public const Int64 Int4Min = -16;
        public const Int64 Int4Max = 127;
        public const Int64 Int8Min = -128;
        public const Int64 Int8Max = -17;
        public const Int64 Int16Min = -32768;
        public const Int64 Int16Max = 32767;
        public const Int64 Int32Min = -2147483648;
        public const Int64 Int32Max = 2147483647;
        public const Int64 Int64Min = -9223372036854775808;
        public const Int64 Int64Max = 9223372036854775807;


        // Text
        public const byte Text4Marker = 0x80;
        public const byte Text8Marker = 0xD0;
        public const byte Text16Marker = 0xD1;
        public const byte Text32Marker = 0xD2;

        // List
        public const byte List4Marker = 0x90;
        public const byte List8Marker = 0xD4;
        public const byte List16Marker = 0xD5;
        public const byte List32Marker = 0xD6;

        // Map
        public const byte Map4Marker = 0xA0;
        public const byte Map8Marker = 0xD8;
        public const byte Map16Marker = 0xD9;
        public const byte Map32Marker = 0xDA;

        // Structure
        public const byte Structure4Marker = 0xB0;
        public const byte Structure8Marker = 0xDC;
        public const byte Structure16Marker = 0xDD;
    }
}
