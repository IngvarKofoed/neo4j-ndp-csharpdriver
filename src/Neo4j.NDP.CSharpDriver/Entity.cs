using System;
using System.Linq;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver
{
    public abstract class Entity : IEntity
    {
        public Entity(string id, IEnumerable<Tuple<string, object>> properties = null)
        {
            if (id == null) throw new ArgumentNullException("id");
            this.Id = id;
            if (properties != null)
            {
                this.Properties = properties.ToDictionary(f => f.Item1, f => f.Item2);
            }
            else {
                this.Properties = new Dictionary<string, object>();
            }
        }

        public abstract EntityType EntityType { get; }

        public string Id { get; private set; }

        public IReadOnlyDictionary<string, object> Properties { get; private set; }

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

