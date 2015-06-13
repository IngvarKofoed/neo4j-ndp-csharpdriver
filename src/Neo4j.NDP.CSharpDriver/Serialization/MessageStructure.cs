using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Represents a structure in a PackStream message.
    /// </summary>
    /// <remarks>This is an immutable class.</remarks>
    public class MessageStructure : IMessageStructure
    {
        /// <summary>
        /// Instantiates an instance of <see cref="MessageStructure"/> with the signature <paramref name="signature"/>
        /// and no fields.
        /// </summary>
        /// <param name="signature">The signature of the structure message objects.</param>
        public MessageStructure(StructureSignature signature)
        {
            this.Signature = signature;
            this.Fields = new List<IMessageObject>();
        }

        /// <summary>
        /// Instantiates an instance of <see cref="MessageStructure"/> with the signature <paramref name="signature"/>
        /// and one field given by <paramref name="field"/>.
        /// </summary>
        /// <param name="signature">The signature of the structure message objects.</param>
        /// <param name="field">The field of this structure message objects.</param>
        public MessageStructure(StructureSignature signature, IMessageObject field)
        {
            if (field == null) throw new ArgumentNullException("field");

            this.Signature = signature;
            this.Fields = new List<IMessageObject> { field };
        }

        /// <summary>
        /// Instantiates an instance of <see cref="MessageStructure"/> with the signature <paramref name="signature"/>
        /// and the fields given by <paramref name="fields"/>.
        /// </summary>
        /// <param name="signature">The signature of the structure message objects.</param>
        /// <param name="field">The fields of this structure message objects.</param>
        public MessageStructure(StructureSignature signature, IEnumerable<IMessageObject> fields)
        {
            if (fields == null) throw new ArgumentNullException("fields");

            this.Signature = signature;
            this.Fields = fields.ToList();
        }

        /// <summary>
        /// This has the type <see cref="MessageObjectType.Structure"/>
        /// </summary>
        public MessageObjectType Type { get { return MessageObjectType.Structure; } }

        /// <summary>
        /// The signature of this structure message objects. <see cref="StructureSignature"/>.
        /// </summary>
        public StructureSignature Signature { get; private set; }

        /// <summary>
        /// The fields of the structure message objects.
        /// </summary>
        public IReadOnlyList<IMessageObject> Fields { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Signature);
            sb.Append("{ ");
            bool isFirst = true;
            foreach (IMessageObject fieldMessageObject in Fields)
            {
                if (!isFirst)
                    sb.Append(", ");
                else
                    isFirst = false;
                sb.Append(fieldMessageObject.ToString());
            }
            sb.Append(" }");
            return sb.ToString();
        }
    }
}
