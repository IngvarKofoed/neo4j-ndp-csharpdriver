using System;

namespace Neo4j.NDP.CSharpDriver
{
    public class Relationship : Entity, IRelationship
    {
        public Relationship(long id, INode startNode, INode endNode) :
            base(id)
        {
            this.StartNode = startNode;
            this.EndNode = endNode;
        }

        public INode StartNode { get; private set; }
        public INode EndNode { get; private set; }
    }
}

