using System;

namespace Neo4j.NDP.CSharpDriver
{
    public interface IEntity
    {
        /// <summary>
        /// The id of the entity (<see cref="INode"/>  or <see cref="IRelationship"/>)
        /// </summary>
        /// <value>The identifier.</value>
        long Id { get; }
    }
}

