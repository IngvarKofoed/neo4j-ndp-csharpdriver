using System;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver
{
    /// <summary>
    /// Base interface for the Neo4j entities <see cref="INode"/>, <see cref="IRelationship"/>.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The type of the entity. <see cref="EntityType"/>.
        /// </summary>
        EntityType EntityType { get; }
        
        /// <summary>
        /// The id of the entity (<see cref="INode"/>  or <see cref="IRelationship"/>)
        /// </summary>
        /// <value>The identifier.</value>
        string Id { get; }

        /// <summary>
        /// Contains the properties of the entity.
        /// </summary>
        IReadOnlyDictionary<string, object> Properties { get; }
    }
}

