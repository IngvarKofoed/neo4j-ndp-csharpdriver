using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4j.NDP.CSharpDriver.Serialization;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Neo4j.NDP.CSharpDriver.Test.Serialization
{
    [TestClass]
    public class MessageObjectConversionExtensionsTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToBoolArgumentNullTest()
        {
            // Run
            MessageObjectConversionExtensions.ToBool(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ToBoolWrongArgumentTest()
        {
            //Run
            MessageObjectConversionExtensions.ToBool(new MessageInt(10));
        }

        [TestMethod]
        public void ToBoolTest()
        {
            // Run
            bool result = MessageObjectConversionExtensions.ToBool(new MessageBool(true));

            // Validate
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDoubleArgumentNullTest()
        {
            // Run
            MessageObjectConversionExtensions.ToDouble(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ToDoubleWrongArgumentTest()
        {
            //Run
            MessageObjectConversionExtensions.ToDouble(new MessageInt(10));
        }

        [TestMethod]
        public void ToDoubleTest()
        {
            // Run
            double result = MessageObjectConversionExtensions.ToDouble(new MessageDouble(42.0));

            // Validate
            Assert.AreEqual(42.0, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToIntArgumentNullTest()
        {
            // Run
            MessageObjectConversionExtensions.ToInt(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ToIntWrongArgumentTest()
        {
            //Run
            MessageObjectConversionExtensions.ToInt(new MessageBool(false));
        }

        [TestMethod]
        public void ToIntTest()
        {
            // Run
            Int64 result = MessageObjectConversionExtensions.ToInt(new MessageInt(42));

            // Validate
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToStringArgumentNullTest()
        {
            // Run
            MessageObjectConversionExtensions.ToString(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ToStringWrongArgumentTest()
        {
            //Run
            MessageObjectConversionExtensions.ToString(new MessageInt(10));
        }

        [TestMethod]
        public void ToStringTest()
        {
            // Run
            string result = MessageObjectConversionExtensions.ToString(new MessageText("test"));

            // Validate
            Assert.AreEqual("test", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToNodeArgumentNullTest()
        {
            // Run
            MessageObjectConversionExtensions.ToNode(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ToNodeWrongArgumentTest()
        {
            //Run
            MessageObjectConversionExtensions.ToNode(new MessageInt(10));
        }

        [TestMethod]
        public void ToNodeTest()
        {
            // Initialize
            MessageStructure nodeTest = new MessageStructure(StructureSignature.Node, new IMessageObject[] { 
                new MessageText("myId"), 
                new MessageList(new MessageText("myLabel")),
                new MessageMap(new Dictionary<IMessageObject, IMessageObject> {
                    { 
                        new MessageText("mapFrom"), new MessageText("mapTo") 
                    }
                })
            });

            // Run
            INode result = MessageObjectConversionExtensions.ToNode(nodeTest);

            // Validate
            Assert.AreEqual("myId", result.Id);
            Assert.AreEqual(1, result.Labels.Count);
            Assert.AreEqual("myLabel", result.Labels[0]);
            Assert.AreEqual(1, result.Properties.Count);
            Assert.AreEqual("mapFrom", result.Properties.Keys.Single());
            Assert.AreEqual("mapTo", result.Properties.Values.Single());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToRelationshipArgumentNullTest()
        {
            // Run
            MessageObjectConversionExtensions.ToRelationship(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ToRelationshipWrongArgumentTest()
        {
            //Run
            MessageObjectConversionExtensions.ToRelationship(new MessageInt(10));
        }

        [TestMethod]
        public void ToRelationshipTest()
        {
            // Initialize
            IMessageObject testRelationship = new MessageStructure(StructureSignature.Relationship, new IMessageObject[] { 
                new MessageText("myId"), 
                new MessageText("myStart"), 
                new MessageText("myEnd"), 
                new MessageText("myType"), 
                new MessageMap(new Dictionary<IMessageObject, IMessageObject> {
                    { 
                        new MessageText("mapFrom"), new MessageText("mapTo") 
                    }
                })
            });

            // Run
            IRelationship result = MessageObjectConversionExtensions.ToRelationship(testRelationship);

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToNodeLabelsArgumentNullTest()
        {
            // Run
            MessageObjectConversionExtensions.ToNodeLabels(null).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ToNodeLabelsWrongArgumentTest()
        {
            // Run
            MessageObjectConversionExtensions.ToNodeLabels(new MessageInt(10)).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToNodeLabelsWrongTypeTest()
        {
            // Run
            MessageObjectConversionExtensions.ToNodeLabels(new MessageList(new MessageInt(10))).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToEntityPropertiesArgumentNullTest()
        {
            // Run
            MessageObjectConversionExtensions.ToEntityProperties(null).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ToEntityPropertiesWrongArgumentTest()
        {
            // Run
            MessageObjectConversionExtensions.ToEntityProperties(new MessageInt(10)).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToEntityPropertiesWrongKeyTypetTest()
        {
            IMessageObject testMap = new MessageMap(new Dictionary<IMessageObject, IMessageObject>
            {
                { new MessageInt(10), new MessageText("test")}
            });

            // Run
            MessageObjectConversionExtensions.ToEntityProperties(testMap).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToEntityPropertiesWrongValueTypeTest()
        {
            IMessageObject testMap = new MessageMap(new Dictionary<IMessageObject, IMessageObject>
            {
                { new MessageText("test"), new MessageList() }
            });

            // Run
            MessageObjectConversionExtensions.ToEntityProperties(testMap).ToList();
        }

        [TestMethod]
        public void ToEntityPropertiesBoolValueTest()
        {
            IMessageObject testMap = new MessageMap(new Dictionary<IMessageObject, IMessageObject>
            {
                { new MessageText("test"), new MessageBool(true) }
            });

            // Run
            List<Tuple<string, object>> result = MessageObjectConversionExtensions.ToEntityProperties(testMap).ToList();

            // Validate
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("test", result[0].Item1);
            Assert.AreEqual(true, (bool)result[0].Item2);
        }

        [TestMethod]
        public void ToEntityPropertiesDoubleValueTest()
        {
            IMessageObject testMap = new MessageMap(new Dictionary<IMessageObject, IMessageObject>
            {
                { new MessageText("test"), new MessageDouble(42.0) }
            });

            // Run
            List<Tuple<string, object>> result = MessageObjectConversionExtensions.ToEntityProperties(testMap).ToList();

            // Validate
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("test", result[0].Item1);
            Assert.AreEqual(42.0, (double)result[0].Item2);
        }

        [TestMethod]
        public void ToEntityPropertiesIntValueTest()
        {
            IMessageObject testMap = new MessageMap(new Dictionary<IMessageObject, IMessageObject>
            {
                { new MessageText("test"), new MessageInt(10) }
            });

            // Run
            List<Tuple<string, object>> result = MessageObjectConversionExtensions.ToEntityProperties(testMap).ToList();

            // Validate
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("test", result[0].Item1);
            Assert.AreEqual(10, (Int64)result[0].Item2);
        }
    }
}
