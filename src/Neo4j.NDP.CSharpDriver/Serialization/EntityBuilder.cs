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

                    var labels = BuildLabels(labelMessageList);
                    var properties = BuildProperties(propertiesMessageMap);

                    INode node = new Node(id, labels, properties);
                    yield return node;
                }
                else if (field.IsStructureWithSignature(StructureSignature.Relationship))
                {
                    string id = fieldStructure.TryGetField<IMessageText>(0).Text;
                    string startNode = fieldStructure.TryGetField<IMessageText>(1).Text;
                    string endNode = fieldStructure.TryGetField<IMessageText>(2).Text;
                    string type = fieldStructure.TryGetField<IMessageText>(3).Text;
                    IMessageMap propertiesMessageMap = fieldStructure.TryGetField<IMessageMap>(4);

                    var properties = BuildProperties(propertiesMessageMap);

                    IRelationship relationship = new Relationship(id, startNode, endNode, type, properties);
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

        private IEnumerable<Tuple<string, object>> BuildProperties(IMessageMap propertiesMessageMap)
        {
            foreach (var keyValue in propertiesMessageMap.Map)
            {
                string key = GetPropertyKey(keyValue.Key);
                object value = GetPropertyValue(keyValue.Value);
             
                yield return new Tuple<string, object>(key, value);
            }
        }

        private string GetPropertyKey(IMessageObject propertyValue)
        {
            if (propertyValue.Type == MessageObjectType.Text)
            {
                return ((MessageText)propertyValue).Text;
            }
            else 
            {
                throw new InvalidOperationException("Unexpected type for map key: " + propertyValue.Type);
            }
        }

        private object GetPropertyValue(IMessageObject propertyValue)
        {
            if (propertyValue.Type == MessageObjectType.Text)
            {
                return ((MessageText)propertyValue).Text;
            }
            else 
            {
                throw new InvalidOperationException("Unexpected type for map value: " + propertyValue.Type);
            }
        }
    }
}

