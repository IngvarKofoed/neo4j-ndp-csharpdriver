using System;
using System.Linq;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver
{
    public class Graph : IGraph
    {
        public Graph(IEnumerable<INode> nodes, IEnumerable<IRelationship> relationships)
        {
            if (nodes == null) throw new ArgumentNullException("nodes");
            if (relationships == null) throw new ArgumentNullException("relationships");
                
            this.Nodes = nodes.ToList();
            this.Relationships = relationships.ToList();
        }
            
        public IEnumerable<INode> Nodes { get; private set; }

        public IEnumerable<IRelationship> Relationships { get; private set; }
    }
}

