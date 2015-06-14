using System;

namespace Neo4j.NDP.CSharpDriver
{
    public class Entity
    {
        public Entity(long id)
        {
            this.Id = id;
        }

        public long Id { get; private set; }
    }
}

