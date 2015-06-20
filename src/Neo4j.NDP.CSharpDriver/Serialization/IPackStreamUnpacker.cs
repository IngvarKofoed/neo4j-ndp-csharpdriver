using System;
using System.IO;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Reads higher level types from a PackStream.
    /// </summary>
    public interface IPackStreamUnpacker
    {
        /// <summary>
        /// Reads the next type (one byte) from the given <paramref name="stream"/>.
        /// </summary>
        /// <returns>Returns the next type and in some cases (bools, null and tiny) the value is also returned.</returns>
        /// <param name="stream">The stream to read the next type from.</param>
        PackStreamUnpackerResult ReadNextType(Stream stream);

        /// <summary>
        /// Reads the next 8 bit signed integer from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="steam">The stream to read the 8 bit signed integer from.</param>
        /// <returns>The read 8 bit signed integer.</returns>
        int ReadInt8(Stream steam);

        /// <summary>
        /// Reads the next 16 bit signed integer from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="steam">The stream to read the 16 bit signed integer from.</param>
        /// <returns>The read 16 bit signed integer.</returns>
        int ReadInt16(Stream steam);

        /// <summary>
        /// Reads the next 32 bit signed integer from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="steam">The stream to read the 32 bit signed integer from.</param>
        /// <returns>The read 32 bit signed integer.</returns>
        int ReadInt32(Stream steam);

        /// <summary>
        /// Reads the next 64 bit signed integer from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="steam">The stream to read the 64 bit signed integer from.</param>
        /// <returns>The read 64 bit signed integer.</returns>
        Int64 ReadInt64(Stream steam);

        /// <summary>
        /// Reads a text with the given <paramref name="length"/> from the given <paramref name="stream"/>.
        /// </summary>
        /// <returns>Returns the read text.</returns>
        /// <param name="stream">The stream to read the text from.</param>
        /// <param name="length">The length of the text to read.</param>
        string ReadText(Stream stream, int length);

        /// <summary>
        /// Reads the structure signature from the given <paramref name="stream"/>.
        /// </summary>
        /// <returns>Returns the read structure signature.</returns>
        /// <param name="stream">The stream to read the structure signature from.</param>
        StructureSignature ReadStructureSignature(Stream stream);
    }
}

