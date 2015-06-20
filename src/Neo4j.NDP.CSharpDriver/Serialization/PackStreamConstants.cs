using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public static class PackStreamConstants
    {
        // Integers markers and constants
        // Note: Int4 has no marker but has the raw value of [-16, 127] or in hex: 0xF0-0xFF and 0x00-0x7F
        public const byte Int8Marker = 0xC8;
        public const byte Int16Marker = 0xC9;
        public const byte Int32Marker = 0xCA;
        public const byte Int64Marker = 0xCB;

        public const byte Int4Min = (byte)(256 - 16);
        public const byte Int4Max = (byte)127;
    }
}
