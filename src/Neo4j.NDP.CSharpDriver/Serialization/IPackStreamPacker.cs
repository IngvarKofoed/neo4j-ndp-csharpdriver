using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// This builds an byte array in the PackStream format.
    /// It is building it like the System.Text.StringBuilder.
    /// </summary>
    public interface IPackStreamPacker
    {
        /// <summary>
        /// Appends an null value to the result byte array.
        /// </summary>
        void AppendNull();

        /// <summary>
        /// Appends the given <paramref name="value"/> bool balue to the result byte array.
        /// </summary>
        /// <param name="value">The bool value to append.</param>
        void Append(bool value);

        /// <summary>
        /// Appends the given <paramref name="text"/> UTF8 encoded, to the result byte array.
        /// </summary>
        /// <param name="text">The text to append.</param>
        void Append(string text);

        /// <summary>
        /// Appends a list header to the result byte array.
        /// A total of <paramref name="listElementCount"/> items are expected to be added.
        /// </summary>
        /// <param name="listElementCount">The number of expted items to be added after the header.</param>
        void AppendListHeader(int listElementCount);

        /// <summary>
        /// Appends a map header to the result byte array.
        /// A total of <paramref name="mapElementCount"/> maps are expected to be added.
        /// </summary>
        /// <param name="listElementCount">The number of expted maps to be added after the header.</param>
        void AppendMapHeader(int mapElementCount);

        /// <summary>
        /// Appends a structure header to the result byte array.
        /// A total of <paramref name="mapElementCount"/> fields are expected to be added.
        /// </summary>
        /// <param name="signature">The <see cref="Signature"/> of the structure.</param>
        /// <param name="fieldInStructureCount">The number of expted fields to be added after the header.</param>
        void AppendStructureHeader(StructureSignature signature, int fieldInStructureCount);

        /// <summary>
        /// Retuns the result byte array.
        /// </summary>
        /// <returns>Retuns the result byte array.</returns>
        byte[] GetBytes();
    }
}
