using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// A message object is used for creating a message to send or
    /// when receving and deserializing PackStream messages.
    /// See the following message object interfaces and classes for more information:
    /// <list type="bullet">
    /// <item><see cref=""/></item>
    /// </list>
    /// </summary>
    public interface IMessageObject
    {
        /// <summary>
        /// The type of the message object. See <see cref="MessageObjectType"/>.
        /// </summary>
        MessageObjectType Type { get; }
    }
}
