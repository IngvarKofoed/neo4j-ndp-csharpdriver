﻿using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Pack stream unpacker result when reading from a stream.
    /// </summary>
    public sealed class PackStreamUnpackerResult
    {
        private PackStreamUnpackerResult()
        {
            this.Length = null;
            this.BoolValue = null;

        }
        /// <summary>
        /// Instantiates a new instance of <see cref="PackStreamUnpackerResult"/>.
        /// </summary>
        /// <param name="type">The <see cref="PackStreamType"/> of the result.</param>
        public PackStreamUnpackerResult(PackStreamType type) :
            this()
        {
            this.Type = type;
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="PackStreamUnpackerResult"/>.
        /// </summary>
        /// <param name="type">The <see cref="PackStreamType"/> of the result.</param>
        /// <param name="length">The length of the pack stream type.</param>
        public PackStreamUnpackerResult(PackStreamType type, int length) :
            this()
        {
            this.Type = type;
            this.Length = length;
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="PackStreamUnpackerResult"/>.
        /// </summary>
        /// <param name="type">The <see cref="PackStreamType"/> of the result.</param>
        /// <param name="boolValue">The value of the read bool.</param>
        public PackStreamUnpackerResult(PackStreamType type, bool boolValue) :
            this()
        {
            this.Type = type;
            this.BoolValue = boolValue;
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

        /// <summary>
        /// If the read type is bool (true/false) then this contains the value.
        /// </summary>
        public bool? BoolValue { get; private set; }
    }
}

