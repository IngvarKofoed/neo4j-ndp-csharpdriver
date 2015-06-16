using System;

namespace Neo4j.NDP.CSharpDriver
{
    /// <summary>
    /// The possible types for <see cref="IEntity"/>s
    /// </summary>
    public enum EntityType
    {
        /// <summary>
        /// The entity is a <see cref="INode"/>
        /// </summary>
        Node = 0,

        /// <summary>
        /// The entity is a <see cref="IRelationship"/>
        /// </summary>
        Relationship = 1
    }
}

