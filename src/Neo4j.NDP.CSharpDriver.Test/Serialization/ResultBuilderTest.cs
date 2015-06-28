using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4j.NDP.CSharpDriver.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neo4j.NDP.CSharpDriver.Test.Serialization
{
    /// <summary>
    /// Builds results of type <typeparamref name="T" /> from records items in the form of <see cref="IMessageList"/>
    /// </summary>
    public interface IResultBuilder<T>
    {
        /// <summary>
        /// Builds results of type <typeparamref name="T" /> from records items in the form of <see cref="IMessageList"/>
        /// </summary>
        /// <param name="recordItems">Record items to build the result of.</param>
        /// <typeparam name="T">
        /// The type to construct through its constructor with the values
        /// of <paramref name="recordItems"/> as arguments to the constructor.
        /// </typeparam>
        T Build(IMessageList recordItems);
    }
    /// <summary>
    /// Builds results of type <typeparamref name="T" /> from records items in the form of <see cref="IMessageList"/>
    /// </summary>
    public class ResultBuilder<T> : IResultBuilder<T>
    {
        private bool hasBeenInitialized = false;
        private Func<object[], object> resultFactory = null;
        private Func<IMessageObject, object>[] casters = null;

        /// <summary>
        /// Builds results of type <typeparamref name="T" /> from records items in the form of <see cref="IMessageList"/>
        /// </summary>
        /// <param name="recordItems">Record items to build the result of.</param>
        /// <typeparam name="T">
        /// The type to construct through its constructor with the values
        /// of <paramref name="recordItems"/> as arguments to the constructor.
        /// </typeparam>
        public T Build(IMessageList recordItems)
        {
            if (recordItems == null) throw new ArgumentNullException("recordItems");

            if (!hasBeenInitialized)
            {
                Initialize(recordItems);
            }

            if (casters.Length != recordItems.Items.Count) throw new InvalidOperationException("Unexpected number of record items received");

            object[] parms = new object[recordItems.Items.Count];
            for (int i = 0; i < parms.Length; i++)
            {
                parms[i] = casters[i](recordItems.Items[i]);
            }

            return (T)resultFactory(parms);
        }

        private void Initialize(IMessageList recordItems)
        {
            Type[] arguments = GetArgumentsAndResultFactory();

            ValidateArgumentsAndBuildCastTable(arguments, recordItems);

            hasBeenInitialized = true;
        }


        private void ValidateArgumentsAndBuildCastTable(Type[] arguments, IMessageList recordItems)
        {
            if (arguments.Length != recordItems.Items.Count) throw new InvalidOperationException(string.Format("Returned record item count does not match the type {0} constructor item count", typeof(T).FullName));

            int length = arguments.Length;

            casters = new Func<IMessageObject, object>[length];

            for (int i = 0; i < length; i++)
            {
                Func<IMessageObject, object> caster = MatchTypes(arguments[i], recordItems.Items[i]);

                if (caster == null) throw new InvalidOperationException(string.Format("Argument of type {0} does not match record item with type {1}", arguments[i].FullName, recordItems.Items[i].Type));

                casters[i] = caster;
            }
        }


        private Func<IMessageObject, object> MatchTypes(Type type, IMessageObject messageObject)
        {
            if (type == typeof(bool) && messageObject is IMessageBool)
            {
                return mo => MessageObjectConversionExtensions.ToBool(mo);
            }
            else if (type == typeof(double) && messageObject is IMessageDouble)
            {
                return mo => MessageObjectConversionExtensions.ToDouble(mo);
            }
            else if (type == typeof(Int64) && messageObject is IMessageInt)
            {
                return mo => MessageObjectConversionExtensions.ToInt(mo);
            }
            else if (type == typeof(int) && messageObject is IMessageInt)
            {
                return mo => (int)MessageObjectConversionExtensions.ToInt(mo);
            }
            else if (type == typeof(string) && messageObject is IMessageText)
            {
                return mo => MessageObjectConversionExtensions.ToString(mo);
            }
            else if (type == typeof(INode) && messageObject is IMessageStructure &&
                     (messageObject as IMessageStructure).Signature == StructureSignature.Node)
            {
                return mo => MessageObjectConversionExtensions.ToNode(mo);
            }
            else if (type == typeof(IRelationship) && messageObject is IMessageStructure &&
                (messageObject as IMessageStructure).Signature == StructureSignature.Relationship)
            {
                return mo => MessageObjectConversionExtensions.ToRelationship(mo);
            }
            else
            {
                return null;
            }
        }


        private Type[] GetArgumentsAndResultFactory()
        {
            if (typeof(T) == typeof(bool))
            {
                resultFactory = parms => (bool)parms[0];
                return new Type[] { typeof(bool) };
            }
            else if (typeof(T) == typeof(double))
            {
                resultFactory = parms => (double)parms[0];
                return new Type[] { typeof(double) };
            }
            else if (typeof(T) == typeof(int))
            {
                resultFactory = parms => (int)parms[0];
                return new Type[] { typeof(int) };
            }
            else if (typeof(T) == typeof(Int64))
            {
                resultFactory = parms => (Int64)parms[0];
                return new Type[] { typeof(Int64) };
            }
            else if (typeof(T) == typeof(string))
            {
                resultFactory = parms => (string)parms[0];
                return new Type[] { typeof(string) };
            }
            else if (typeof(T) == typeof(INode))
            {
                resultFactory = parms => (INode)parms[0];
                return new Type[] { typeof(INode) };
            }
            else if (typeof(T) == typeof(IRelationship))
            {
                resultFactory = parms => (IRelationship)parms[0];
                return new Type[] { typeof(IRelationship) };
            }
            else
            {
                IEnumerable<ConstructorInfo> constructors = typeof(T).
                    GetConstructors(BindingFlags.Public | BindingFlags.Instance).
                    Where(f => f.GetParameters().Length > 0);

                if (constructors.Count() == 0) throw new InvalidOperationException(string.Format("The type {0} does not have any constructors taking 1 or more arugments", typeof(T).FullName));
                if (constructors.Count() > 1) throw new InvalidOperationException(string.Format("The type {0} has more than 1 constructors taking 1 or more arugments", typeof(T).FullName));

                ConstructorInfo constructor = constructors.Single();

                resultFactory = parms => constructor.Invoke(parms);
                return constructor.GetParameters().Select(f => f.ParameterType).ToArray();
            }
        }
    }

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

        public static IEnumerable<Tuple<string, object>> ToEntityProperties(this IMessageObject messageObject)
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




    [TestClass]
    public class ResultBuilderTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgumentTest()
        {
            // Initialize
            IResultBuilder<int> resultBuilder = new ResultBuilder<int>();

            // Run 
            resultBuilder.Build(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MissingConstructoresTest()
        {
            // Initialize
            IResultBuilder<MissingConstructoresTestClass> resultBuilder = new ResultBuilder<MissingConstructoresTestClass>();

            // Run 
            resultBuilder.Build(new MessageList(new MessageInt(10)));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TooManyConstructoresTest()
        {
            // Initialize
            IResultBuilder<ToManyConstructoresTestClass> resultBuilder = new ResultBuilder<ToManyConstructoresTestClass>();

            // Run 
            resultBuilder.Build(new MessageList(new MessageInt(10)));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WrongArgumentCountTest()
        {
            // Initialize
            IResultBuilder<Tuple<int, int>> resultBuilder = new ResultBuilder<Tuple<int, int>>();

            // Run 
            resultBuilder.Build(new MessageList(new MessageInt(10)));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WrongTypeTest()
        {
            // Initialize
            IResultBuilder<Exception> resultBuilder = new ResultBuilder<Exception>();

            // Run 
            resultBuilder.Build(new MessageList(new MessageInt(10)));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TypeMisMatchTest()
        {
            // Initialize
            IResultBuilder<bool> resultBuilder = new ResultBuilder<bool>();

            // Run 
            resultBuilder.Build(new MessageList(new MessageInt(10)));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RecordItemCountMisMatchTest()
        {
            // Initialize
            IResultBuilder<Tuple<int, int>> resultBuilder = new ResultBuilder<Tuple<int, int>>();
            resultBuilder.Build(new MessageList(new IMessageObject[] {
                new MessageInt(10),
                new MessageInt(10)
            }));

            // Run 
            resultBuilder.Build(new MessageList(new MessageInt(10)));
        }

        [TestMethod]
        public void BoolValueTest()
        {
            // Initialize
            IResultBuilder<bool> resultBuilder = new ResultBuilder<bool>();

            // Run 
            bool result = resultBuilder.Build(new MessageList(new MessageBool(true)));

            // Validate
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoubleValueTest()
        {
            // Initialize
            const double testValue = 43.42;
            IResultBuilder<double> resultBuilder = new ResultBuilder<double>();

            // Run 
            double result = resultBuilder.Build(new MessageList(new MessageDouble(testValue)));

            // Validate
            Assert.AreEqual(testValue, result);
        }

        [TestMethod]
        public void IntValueTest()
        {
            // Initialize
            const int testValue = 43;
            IResultBuilder<int> resultBuilder = new ResultBuilder<int>();

            // Run 
            int result = resultBuilder.Build(new MessageList(new MessageInt(testValue)));

            // Validate
            Assert.AreEqual(testValue, result);
        }

        [TestMethod]
        public void Int64ValueTest()
        {
            // Initialize
            const Int64 testValue = 5420000000;
            IResultBuilder<Int64> resultBuilder = new ResultBuilder<Int64>();

            // Run 
            Int64 result = resultBuilder.Build(new MessageList(new MessageInt(testValue)));

            // Validate
            Assert.AreEqual(testValue, result);
        }

        [TestMethod]
        public void StringValueTest()
        {
            // Initialize
            const string testValue = "test";
            IResultBuilder<string> resultBuilder = new ResultBuilder<string>();

            // Run 
            string result = resultBuilder.Build(new MessageList(new MessageText(testValue)));

            // Validate
            Assert.AreEqual(testValue, result);
        }

        [TestMethod]
        public void NodeValueTest()
        {
            // Initialize
            IResultBuilder<INode> resultBuilder = new ResultBuilder<INode>();
            IMessageList messageList = new MessageList(
                new MessageStructure(StructureSignature.Node, new IMessageObject[] { 
                    new MessageText("myId"), 
                    new MessageList(new MessageText("myLabel")),
                    new MessageMap(new Dictionary<IMessageObject, IMessageObject> {
                        { 
                            new MessageText("mapFrom"), new MessageText("mapTo") 
                        }
                    })
                })
            );

            // Run 
            INode result = resultBuilder.Build(messageList);

            // Validate
            Assert.AreEqual("myId", result.Id);
            Assert.AreEqual(1, result.Labels.Count);
            Assert.AreEqual("myLabel", result.Labels[0]);
            Assert.AreEqual(1, result.Properties.Count);
            Assert.AreEqual("mapFrom", result.Properties.Keys.Single());
            Assert.AreEqual("mapTo", result.Properties.Values.Single());
        }

        [TestMethod]
        public void RelationshipValueTest()
        {
            // Initialize
            IResultBuilder<IRelationship> resultBuilder = new ResultBuilder<IRelationship>();
            IMessageList messageList = new MessageList(
                new MessageStructure(StructureSignature.Relationship, new IMessageObject[] { 
                    new MessageText("myId"), 
                    new MessageText("myStart"), 
                    new MessageText("myEnd"), 
                    new MessageText("myType"), 
                    new MessageMap(new Dictionary<IMessageObject, IMessageObject> {
                        { 
                            new MessageText("mapFrom"), new MessageText("mapTo") 
                        }
                    })
                })
            );

            // Run 
            IRelationship result = resultBuilder.Build(messageList);

            // Validate
            Assert.AreEqual("myId", result.Id);
            Assert.AreEqual("myStart", result.StartNodeId);
            Assert.AreEqual("myEnd", result.EndNodeId);
            Assert.AreEqual("myType", result.Type);
            Assert.AreEqual(1, result.Properties.Count);
            Assert.AreEqual("mapFrom", result.Properties.Keys.Single());
            Assert.AreEqual("mapTo", result.Properties.Values.Single());
        }

        [TestMethod]
        public void CompositeValueTest()
        {
            // Initialize
            const int testValue1 = 42;
            const bool testValue2 = true;
            IResultBuilder<Tuple<int, bool>> resultBuilder = new ResultBuilder<Tuple<int, bool>>();

            // Run 
            Tuple<int, bool> result = resultBuilder.Build(new MessageList(new IMessageObject[] {
                new MessageInt(testValue1), 
                new MessageBool(testValue2)
            }));

            // Validate
            Assert.AreEqual(testValue1, result.Item1);
            Assert.AreEqual(testValue2, result.Item2);
        }


        class MissingConstructoresTestClass
        {
            public MissingConstructoresTestClass() { }
        }

        class ToManyConstructoresTestClass
        {
            public ToManyConstructoresTestClass(int i1) { }
            public ToManyConstructoresTestClass(int i1, int i2) { }
        }
    }
}
