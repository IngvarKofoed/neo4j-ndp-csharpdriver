using System;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver
{
    // TODO: This interface is not finished yet!
    public interface IConnection : IDisposable
    {
        void Run(string statement, IDictionary<string, object> parameters = null);

        IEnumerable<T> Run<T>(string statement, IDictionary<string, object> parameters = null);
    }
}
