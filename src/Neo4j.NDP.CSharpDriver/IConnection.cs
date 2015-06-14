using System;

namespace Neo4j.NDP.CSharpDriver
{
    // TODO: This interface is not finished yet!
    public interface IConnection : IDisposable
    {
        IGraph Run(string statement);
    }
}
