using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Converts from/to little endian (target) to platform endian.
    /// </summary>
    public class LittleEndianTargetBitConverter : BitConverterBase
    {
        /// <summary>
        /// Converts the bytes to little endian.
        /// </summary>
        /// <param name="bytes">The bytes to convert.</param>
        /// <returns>The bytes converted to little endian.</returns>
        protected override byte[] ToTargetEndian(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                return bytes;
            }
            else
            {
                return bytes.Reverse().ToArray();
            }
        }

        /// <summary>
        /// Converts the bytes to the platform endian type.
        /// </summary>
        /// <param name="bytes">The bytes to convert.</param>
        /// <returns>The bytes converted to the platform endian type.</returns>
        protected override byte[] ToPlatformEndian(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                return bytes;
            }
            else
            {
                return bytes.Reverse().ToArray();
            }
        }
    }
}
