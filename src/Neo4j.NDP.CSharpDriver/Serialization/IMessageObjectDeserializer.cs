using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public interface IMessageObjectDeserializer
    {
        /// <summary>
        /// Deserializes the given <paramref name="data"/> bytes in the PackStream format to a <see cref="IMessageObject"/> tree.
        /// </summary>
        /// <param name="data">The bytes to deserialize from.</param>
        IMessageObject Deserialize(byte[] data);

        /// <summary>
        /// Deserializes the given <paramref name="stream"/> by reading bytes in the PackStream format to a <see cref="IMessageObject"/> tree.
        /// </summary>
        /// <param name="stream">The stream to read the to deserialize bytes from.</param>
        IMessageObject Deserialize(Stream stream);
    }
}
