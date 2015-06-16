using System;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public interface IEntityBuilder
    {
        /// <summary>
        /// Builds <see cref="IEntity"/>' instances from the record structure given by <paramref name="messageStructure"/>.
        /// The <paramref name="messageStructure"/> should be a record containing the needed 
        /// structures and values to build an <see cref="IEntity"/> instances.
        /// If this is not the case an exception is thrown.
        /// </summary>
        /// <returns>The newly build <see cref="IEntity"/> instances.</returns>
        /// <param name="messageStructure">Message structure if the record signature.</param>
        IEnumerable<IEntity> BuildFromRecord(IMessageStructure messageStructure);
    }
}

