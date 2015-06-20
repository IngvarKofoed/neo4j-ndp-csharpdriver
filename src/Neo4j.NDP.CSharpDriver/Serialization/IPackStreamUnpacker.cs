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
        /// Reads the next type from the given <paramref name="stream"/>.
        /// </summary>
        /// <returns>Returns the next type.</returns>
        /// <param name="stream">The stream to read the next type from.</param>
        PackStreamUnpackerResult ReadNextType(Stream stream);

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

