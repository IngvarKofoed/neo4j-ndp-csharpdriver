using System;
using System.Collections.Generic;
using Neo4j.NDP.CSharpDriver.Extensions;

namespace Neo4j.NDP.CSharpDriver
{
    public class Relationship : Entity, IRelationship
    {
        public Relationship(string id, string startNodeId, string endNodeId, string type, IEnumerable<Tuple<string, object>> properties = null) :
        base(id, properties)
        {
            this.StartNodeId = startNodeId;
            this.EndNodeId = endNodeId;
            this.Type = type;
        }

        public override EntityType EntityType { get { return EntityType.Relationship; } }

        public string StartNodeId { get; private set; }
        public string EndNodeId { get; private set; }
        public string Type { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}, {2}-{3}->{4} {5}", EntityType, Id, StartNodeId, Type, EndNodeId, Properties.ToReadableString());
        }
    }
}

