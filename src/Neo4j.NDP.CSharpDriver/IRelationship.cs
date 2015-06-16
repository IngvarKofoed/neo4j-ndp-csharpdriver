using System;

namespace Neo4j.NDP.CSharpDriver
{
    public interface IRelationship : IEntity
    {
        string StartNodeId { get; }

        string EndNodeId { get; }

        string Type { get; }
    }
}

