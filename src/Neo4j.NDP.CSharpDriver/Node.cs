using System;
using System.Linq;
using System.Collections.Generic;
using Neo4j.NDP.CSharpDriver.Extensions;

namespace Neo4j.NDP.CSharpDriver
{
    public class Node : Entity, INode
    {
        public Node(string id, IEnumerable<string> labels = null, IEnumerable<Tuple<string, object>> properties = null) :
            base(id, properties)
        {
            if (labels != null)
            {
                Labels = labels.ToList();
            }
            else 
            {
                Labels = new List<string>();
            }
        }

        public override EntityType EntityType { get { return EntityType.Node; } }

        public IReadOnlyList<string> Labels { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}: {1} {2} {3}", EntityType, Id, Labels.ToReadableString(), Properties.ToReadableString());
        }
    }
}

