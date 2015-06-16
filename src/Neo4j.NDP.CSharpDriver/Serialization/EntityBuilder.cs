using System;
using System.Linq;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public class EntityBuilder : IEntityBuilder
    {
        public IEnumerable<IEntity> BuildFromRecord(IMessageStructure messageStructure)
        {
            IMessageList items = messageStructure.TryGetField<IMessageList>(0);
            if (items == null) throw new InvalidOperationException("Did not find the items of the Node");

            foreach (IMessageObject field in items.Items)
            {
                IMessageStructure fieldStructure = field as IMessageStructure;
                if (field.IsStructureWithSignature(StructureSignature.Node))
                {
                    string id = fieldStructure.TryGetField<IMessageText>(0).Text;
                    IMessageList labelMessageList = fieldStructure.TryGetField<IMessageList>(1);
                    IMessageMap propertiesMessageMap = fieldStructure.TryGetField<IMessageMap>(2);

                    IEnumerable<string> labels = BuildLabels(labelMessageList);

                    INode node = new Node(id, labels);
                    yield return node;
                }
                else if (field.IsStructureWithSignature(StructureSignature.Relationship))
                {
                    string id = fieldStructure.TryGetField<IMessageText>(0).Text;
                    string startNode = fieldStructure.TryGetField<IMessageText>(1).Text;
                    string endNode = fieldStructure.TryGetField<IMessageText>(2).Text;
                    string type = fieldStructure.TryGetField<IMessageText>(3).Text;
                    IRelationship relationship = new Relationship(id, startNode, endNode, type);
                    yield return relationship;
                }
                else 
                {
                    throw new NotImplementedException();
                }
            }

            yield break;
        }

        private IEnumerable<string> BuildLabels(IMessageList labelMessageList)
        {
            foreach (IMessageObject itemObject in labelMessageList.Items)
            {
                if (itemObject.Type != MessageObjectType.Text) throw new InvalidOperationException("Unexpected type for node label: " + itemObject.Type);
            
                IMessageText labelObject = itemObject as IMessageText;
                yield return labelObject.Text;
            }
        }
    }
}

