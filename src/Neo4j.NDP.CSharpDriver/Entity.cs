using System;

namespace Neo4j.NDP.CSharpDriver
{
    public class Entity
    {
        public Entity(string id)
        {
            this.Id = id;
        }

        public string Id { get; private set; }


        public override bool Equals(object obj)
        {
            return Equals(obj as Entity);
        }

        public bool Equals(Entity node)
        {
            if (node == null) return false;
            return node.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}

