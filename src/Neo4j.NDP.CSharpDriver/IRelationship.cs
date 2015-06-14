using System;

namespace Neo4j.NDP.CSharpDriver
{
    public interface IRelationship : IEntity
    {
        INode StartNode { get; }
        INode EndNode { get; }
    }
}

