using System;

namespace Neo4j.NDP.CSharpDriver
{
    public class Relationship : Entity, IRelationship
    {
        public Relationship(string id, string startNodeId, string endNodeId) :
            base(id)
        {
            this.StartNodeId = startNodeId;
            this.EndNodeId = endNodeId;
        }

        public string StartNodeId { get; private set; }
        public string EndNodeId { get; private set; }
    }
}

