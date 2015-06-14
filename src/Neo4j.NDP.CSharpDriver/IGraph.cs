using System;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver
{
    /// <summary>
    /// Contains nodes, relations and paths created from a query.
    /// </summary>
    public interface IGraph
    {
        /// <summary>
        /// The nodes from the query.
        /// </summary>
        IEnumerable<INode> Nodes { get; }

        /// <summary>
        /// The relationships from the query.
        /// </summary>
        IEnumerable<IRelationship> Relationships { get; }
    }
}

