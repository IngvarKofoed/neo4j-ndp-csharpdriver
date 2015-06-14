using System;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver
{
    /// <summary>
    /// Represents a Neo4j graph node.
    /// </summary>
    public interface INode : IEntity
    {
        IReadOnlyList<string> Labels { get; }
    }
}

