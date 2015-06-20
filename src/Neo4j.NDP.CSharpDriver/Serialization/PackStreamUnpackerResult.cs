using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Pack stream unpacker result when reading from a stream.
    /// </summary>
    public sealed class PackStreamUnpackerResult
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="PackStreamUnpackerResult"/>.
        /// </summary>
        /// <param name="type">The <see cref="PackStreamType"/> of the result.</param>
        public PackStreamUnpackerResult(PackStreamType type)
        {
            this.Type = type;
            this.Length = null;
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="PackStreamUnpackerResult"/>.
        /// </summary>
        /// <param name="type">The <see cref="PackStreamType"/> of the result.</param>
        /// <param name="length">The length of the pack stream type.</param>
        public PackStreamUnpackerResult(PackStreamType type, int length)
        {
            this.Type = type;
            this.Length = length;
        }

        /// <summary>
        /// The type of the following data in the pack stream
        /// </summary>
        public PackStreamType Type { get; private set; }

        /// <summary>
        /// If the following data in the pack stream has a length (text, list, map, structure, etc).
        /// This contains the length of that data. Else null.
        /// </summary>
        public int? Length { get; private set; }
    }
}

