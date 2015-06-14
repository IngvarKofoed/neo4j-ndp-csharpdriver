using System;
using System.Linq;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public class GraphBuilder
    {
        private IList<INode> nodes = new List<INode>();
        private IList<IRelationship> relationships = new List<IRelationship>();

        public GraphBuilder()
        {
        }

        public void AddRecord(IMessageStructure messageStructure)
        {
            IMessageList items = messageStructure.TryGetField<IMessageList>(0);
            if (items == null) throw new InvalidOperationException("Did not find the items of the Node");

            foreach (IMessageObject field in items.Items)
            {
                IMessageStructure fieldStructure = field as IMessageStructure;
                if (field.IsStructureWithSignature(StructureSignature.Node))
                {
                    string id = fieldStructure.TryGetField<IMessageText>(0).Text;
                    INode node = new Node(id);
                    nodes.Add(node);
                }
                else if (field.IsStructureWithSignature(StructureSignature.Relationship))
                {
                    string id = fieldStructure.TryGetField<IMessageText>(0).Text;
                    IRelationship relationship = new Relationship(id, "a", "b");
                    relationships.Add(relationship);
                }
                else 
                {
                    throw new NotImplementedException();
                }
            }
        }


        public IGraph GetGraph()
        {
            return new Graph(
                nodes.Distinct(),
                relationships.Distinct()
            );
        }
    }
}

