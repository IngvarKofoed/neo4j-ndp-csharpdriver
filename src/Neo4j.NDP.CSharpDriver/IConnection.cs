using System;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver
{
    // TODO: This interface is not finished yet!
    public interface IConnection : IDisposable
    {
        IEnumerable<IEntity> Run(string statement, IDictionary<string, object> parameters = null);
        }
}
