using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neo4j.NDP.CSharpDriver.Serialization;
using System;
using System.Collections.Generic;


namespace Neo4j.NDP.CSharpDriver.Test.Serialization
{
    [TestClass]
    public class MessageObjectSerializerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullFactoryArgumentTest()
        {
            // Run
            IMessageObjectSerializer serializer = new MessageObjectSerializer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullMessageObjectArgumentTest()
        {
            // Initialie
            Mock<IPackStreamPackerFactory> factory = new Mock<IPackStreamPackerFactory>();
            IMessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);

            // Run
            serializer.Serialize(null);
        }

        [TestMethod]
        public void MessageNullTest()
        {
            // Initialie
            byte[] testBytes = new byte[] { 0xAB };
            Mock<IPackStreamPacker> packer = new Mock<IPackStreamPacker>();
            packer.Setup(f => f.GetBytes()).Returns(testBytes);
            Mock<IPackStreamPackerFactory> factory = new Mock<IPackStreamPackerFactory>();
            factory.Setup(f => f.Create()).Returns(packer.Object);

            IMessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);
            IMessageNull messageText = new MessageNull();

            // Run
            byte[] result = serializer.Serialize(messageText);

            // Validate
            packer.Verify(f => f.AppendNull());
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(testBytes[0], result[0]);
        }

        [TestMethod]
        public void MessageBoolTest()
        {
            // Initialie
            byte[] testBytes = new byte[] { 0xAB };
            Mock<IPackStreamPacker> packer = new Mock<IPackStreamPacker>();
            packer.Setup(f => f.GetBytes()).Returns(testBytes);
            Mock<IPackStreamPackerFactory> factory = new Mock<IPackStreamPackerFactory>();
            factory.Setup(f => f.Create()).Returns(packer.Object);

            IMessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);
            IMessageBool messageText = new MessageBool(false);

            // Run
            byte[] result = serializer.Serialize(messageText);

            // Validate
            packer.Verify(f => f.Append(false));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(testBytes[0], result[0]);
        }

        [TestMethod]
        public void MessageDoubleTest()
        {
            // Initialie
            const double testValue = 1.0;
            byte[] testBytes = new byte[] { 0xAB };
            Mock<IPackStreamPacker> packer = new Mock<IPackStreamPacker>();
            packer.Setup(f => f.GetBytes()).Returns(testBytes);
            Mock<IPackStreamPackerFactory> factory = new Mock<IPackStreamPackerFactory>();
            factory.Setup(f => f.Create()).Returns(packer.Object);

            IMessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);
            IMessageDouble messageText = new MessageDouble(testValue);

            // Run
            byte[] result = serializer.Serialize(messageText);

            // Validate
            packer.Verify(f => f.Append(testValue));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(testBytes[0], result[0]);
        }

        [TestMethod]
        public void MessageIntTest()
        {
            // Initialie
            const int testValue = 100;
            byte[] testBytes = new byte[] { 0xAB };
            Mock<IPackStreamPacker> packer = new Mock<IPackStreamPacker>();
            packer.Setup(f => f.GetBytes()).Returns(testBytes);
            Mock<IPackStreamPackerFactory> factory = new Mock<IPackStreamPackerFactory>();
            factory.Setup(f => f.Create()).Returns(packer.Object);

            IMessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);
            IMessageInt messageText = new MessageInt(testValue);

            // Run
            byte[] result = serializer.Serialize(messageText);

            // Validate
            packer.Verify(f => f.Append((byte)testValue));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(testBytes[0], result[0]);
        }

        [TestMethod]
        public void MessageTextTest()
        {
            // Initialie
            const string testString = "Test";
            byte[] testBytes = new byte[] { 0xAB };
            Mock<IPackStreamPacker> packer = new Mock<IPackStreamPacker>();
            packer.Setup(f => f.GetBytes()).Returns(testBytes);
            Mock<IPackStreamPackerFactory> factory = new Mock<IPackStreamPackerFactory>();
            factory.Setup(f => f.Create()).Returns(packer.Object);

            IMessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);
            IMessageText messageText = new MessageText(testString);

            // Run
            byte[] result = serializer.Serialize(messageText);

            // Validate
            packer.Verify(f => f.Append(testString));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(testBytes[0], result[0]);
        }

        [TestMethod]
        public void MessageListTest()
        {
            // Initialie
            const string testString = "Test";
            byte[] testBytes = new byte[] { 0xAB };
            Mock<IPackStreamPacker> packer = new Mock<IPackStreamPacker>();
            packer.Setup(f => f.GetBytes()).Returns(testBytes);
            Mock<IPackStreamPackerFactory> factory = new Mock<IPackStreamPackerFactory>();
            factory.Setup(f => f.Create()).Returns(packer.Object);

            IMessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);
            IMessageList messageList = new MessageList(new MessageText(testString));

            // Run
            byte[] result = serializer.Serialize(messageList);

            // Validate
            packer.Verify(f => f.AppendListHeader(1));
            packer.Verify(f => f.Append(testString));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(testBytes[0], result[0]);
        }

        [TestMethod]
        public void MessageMapTest()
        {
            // Initialie
            const string testString1 = "Test1";
            const string testString2 = "Test2";
            byte[] testBytes = new byte[] { 0xAB };
            Mock<IPackStreamPacker> packer = new Mock<IPackStreamPacker>();
            packer.Setup(f => f.GetBytes()).Returns(testBytes);
            Mock<IPackStreamPackerFactory> factory = new Mock<IPackStreamPackerFactory>();
            factory.Setup(f => f.Create()).Returns(packer.Object);

            IMessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);
            IMessageMap messageMap = new MessageMap(
                new Dictionary<IMessageObject, IMessageObject> {
                    { new MessageText(testString1), new MessageText(testString2) }
            });

            // Run
            byte[] result = serializer.Serialize(messageMap);

            // Validate
            packer.Verify(f => f.AppendMapHeader(1));
            packer.Verify(f => f.Append(testString1));
            packer.Verify(f => f.Append(testString2));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(testBytes[0], result[0]);
        }

        [TestMethod]
        public void MessageStructureTest()
        {
            // Initialie
            const string testString = "Test";
            byte[] testBytes = new byte[] { 0xAB };
            Mock<IPackStreamPacker> packer = new Mock<IPackStreamPacker>();
            packer.Setup(f => f.GetBytes()).Returns(testBytes);
            Mock<IPackStreamPackerFactory> factory = new Mock<IPackStreamPackerFactory>();
            factory.Setup(f => f.Create()).Returns(packer.Object);

            IMessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);
            IMessageStructure messageStructure = new MessageStructure(
                StructureSignature.Init, 
                new MessageText(testString));

            // Run
            byte[] result = serializer.Serialize(messageStructure);

            // Validate
            packer.Verify(f => f.AppendStructureHeader(StructureSignature.Init, 1));
            packer.Verify(f => f.Append(testString));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(testBytes[0], result[0]);
        }
    }
}
