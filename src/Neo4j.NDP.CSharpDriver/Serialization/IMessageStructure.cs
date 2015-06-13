using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a structure in a PackStream message.
    /// </summary>
    public interface IMessageStructure : IMessageObject
    {
        /// <summary>
        /// The signature of the structure. <see cref="StructureSignature"/>.
        /// </summary>
        StructureSignature Signature { get; }

        /// <summary>
        /// The fields of the message map objects.
        /// </summary>
        IReadOnlyList<IMessageObject> Fields { get; }
    }
}
