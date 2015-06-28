using System;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Converts IMessageObjects to either .NET value types or one of the following:
    /// <see cref="INode"/>, <see cref="IRelatoinship"/>
    /// </summary>
    public static class MessageObjectConversionExtensions
    {
        public static bool ToBool(this IMessageObject messageObject)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");
            if (messageObject.Type != MessageObjectType.Bool) throw new ArgumentException("Expected type: IMessageBool", "messageObject");
            return (messageObject as IMessageBool).Value;
        }

        public static double ToDouble(this IMessageObject messageObject)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");
            if (messageObject.Type != MessageObjectType.Double) throw new ArgumentException("Expected type: IMessageBool", "messageObject");
            return (messageObject as IMessageDouble).Value;
        }

        public static Int64 ToInt(this IMessageObject messageObject)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");
            if (messageObject.Type != MessageObjectType.Int) throw new ArgumentException("Expected type: IMessageBool", "messageObject");
            return (messageObject as IMessageInt).Value;
        }

        public static string ToString(this IMessageObject messageObject)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");
            if (messageObject.Type != MessageObjectType.Text) throw new ArgumentException("Expected type: IMessageBool", "messageObject");
            return (messageObject as IMessageText).Text;
        }

        public static INode ToNode(this IMessageObject messageObject)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");
            IMessageStructure messageStructure = messageObject as IMessageStructure;
            if (messageStructure == null || messageStructure.Signature != StructureSignature.Node) throw new ArgumentException("Expected type: IMessageStructure with signature: " + StructureSignature.Node, "messageObject");

            string id = messageStructure.TryGetField<IMessageText>(0).Text;
            IMessageList labelMessageList = messageStructure.TryGetField<IMessageList>(1);
            IMessageMap propertiesMessageMap = messageStructure.TryGetField<IMessageMap>(2);

            var labels = labelMessageList.ToNodeLabels();
            var properties = propertiesMessageMap.ToEntityProperties();

            return new Node(id, labels, properties);
        }

        public static IRelationship ToRelationship(this IMessageObject messageObject)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");
            IMessageStructure messageStructure = messageObject as IMessageStructure;
            if (messageStructure == null || messageStructure.Signature != StructureSignature.Relationship) throw new ArgumentException("Expected type: IMessageStructure with signature: " + StructureSignature.Relationship, "messageObject");

            string id = messageStructure.TryGetField<IMessageText>(0).Text;
            string startNode = messageStructure.TryGetField<IMessageText>(1).Text;
            string endNode = messageStructure.TryGetField<IMessageText>(2).Text;
            string type = messageStructure.TryGetField<IMessageText>(3).Text;
            IMessageMap propertiesMessageMap = messageStructure.TryGetField<IMessageMap>(4);

            var properties = propertiesMessageMap.ToEntityProperties();

            return new Relationship(id, startNode, endNode, type, properties);
        }

        public static IEnumerable<string> ToNodeLabels(this IMessageObject messageObject)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");
            if (messageObject.Type != MessageObjectType.List) throw new ArgumentException("Expected type: IMessageList", "messageObject");
            IMessageList messageList = (IMessageList)messageObject;

            foreach (IMessageObject itemObject in messageList.Items)
            {
                if (itemObject.Type == MessageObjectType.Text) 
                {
                    IMessageText labelObject = itemObject as IMessageText;
                    yield return labelObject.Text;
                }
                else 
                {
                    throw new InvalidOperationException("Unexpected type for node label: " + itemObject.Type);
                }
            }
        }

        public static IEnumerable<Tuple<string, object>> ToEntityProperties(this IMessageMap messageObject)
        {
            if (messageObject == null) throw new ArgumentNullException("messageObject");
            if (messageObject.Type != MessageObjectType.Map) throw new ArgumentException("Expected type: IMessageList", "messageObject");

            IMessageMap messageMap = (IMessageMap)messageObject;
            foreach (var keyValue in messageMap.Map)
            {
                string key = GetPropertyKey(keyValue.Key);
                object value = GetPropertyValue(keyValue.Value);

                yield return new Tuple<string, object>(key, value);
            }
        }

        private static string GetPropertyKey(IMessageObject propertyValue)
        {
            if (propertyValue.Type == MessageObjectType.Text)
            {
                return ((MessageText)propertyValue).Text;
            }
            else 
            {
                throw new InvalidOperationException("Unexpected type for entity properties map key: " + propertyValue.Type);
            }
        }

        private static object GetPropertyValue(IMessageObject propertyValue)
        {
            if (propertyValue.Type == MessageObjectType.Bool)
            {
                return (propertyValue as IMessageBool).Value;
            }
            else if (propertyValue.Type == MessageObjectType.Double)
            {
                return (propertyValue as IMessageDouble).Value;
            }
            else if (propertyValue.Type == MessageObjectType.Int)
            {
                return (propertyValue as IMessageInt).Value;
            }
            else if (propertyValue.Type == MessageObjectType.Text)
            {
                return (propertyValue as MessageText).Text;
            }
            else 
            {
                throw new InvalidOperationException("Unexpected type for entity properties map value: " + propertyValue.Type);
            }
        }
    }
}

