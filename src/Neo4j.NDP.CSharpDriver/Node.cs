using System;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver
{
    public class Node : Entity, INode
    {
        public Node(string id) :
            base(id)
        {
        }

        public override EntityType Type { get { return EntityType.Node; } }

        public IReadOnlyList<string> Labels { get; private set; }
    }
}

