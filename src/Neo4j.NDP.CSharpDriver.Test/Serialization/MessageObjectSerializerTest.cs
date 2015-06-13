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
            MessageObjectSerializer serializer = new MessageObjectSerializer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullMessageObjectArgumentTest()
        {
            // Initialie
            Mock<IPackSteamBuilderFactory> factory = new Mock<IPackSteamBuilderFactory>();
            MessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);

            // Run
            serializer.Serialize(null);
        }

        [TestMethod]
        public void MessageTextTest()
        {
            // Initialie
            const string testString = "Test";
            byte[] testBytes = new byte[] { 0xAB };
            Mock<IPackStreamBuilder> builder = new Mock<IPackStreamBuilder>();
            builder.Setup(f => f.GetBytes()).Returns(testBytes);
            Mock<IPackSteamBuilderFactory> factory = new Mock<IPackSteamBuilderFactory>();
            factory.Setup(f => f.Create()).Returns(builder.Object);

            MessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);
            MessageText messageText = new MessageText(testString);

            // Run
            byte[] result = serializer.Serialize(messageText);

            // Validate
            builder.Verify(f => f.Append(testString));
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
            Mock<IPackStreamBuilder> builder = new Mock<IPackStreamBuilder>();
            builder.Setup(f => f.GetBytes()).Returns(testBytes);
            Mock<IPackSteamBuilderFactory> factory = new Mock<IPackSteamBuilderFactory>();
            factory.Setup(f => f.Create()).Returns(builder.Object);

            MessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);
            MessageList messageList = new MessageList(new MessageText(testString));

            // Run
            byte[] result = serializer.Serialize(messageList);

            // Validate
            builder.Verify(f => f.AppendListHeader(1));
            builder.Verify(f => f.Append(testString));
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
            Mock<IPackStreamBuilder> builder = new Mock<IPackStreamBuilder>();
            builder.Setup(f => f.GetBytes()).Returns(testBytes);
            Mock<IPackSteamBuilderFactory> factory = new Mock<IPackSteamBuilderFactory>();
            factory.Setup(f => f.Create()).Returns(builder.Object);

            MessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);
            MessageMap messageMap = new MessageMap(
                new Dictionary<IMessageObject, IMessageObject> {
                    { new MessageText(testString1), new MessageText(testString2) }
            });

            // Run
            byte[] result = serializer.Serialize(messageMap);

            // Validate
            builder.Verify(f => f.AppendMapHeader(1));
            builder.Verify(f => f.Append(testString1));
            builder.Verify(f => f.Append(testString2));
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
            Mock<IPackStreamBuilder> builder = new Mock<IPackStreamBuilder>();
            builder.Setup(f => f.GetBytes()).Returns(testBytes);
            Mock<IPackSteamBuilderFactory> factory = new Mock<IPackSteamBuilderFactory>();
            factory.Setup(f => f.Create()).Returns(builder.Object);

            MessageObjectSerializer serializer = new MessageObjectSerializer(factory.Object);
            MessageStructure messageStructure = new MessageStructure(
                StructureSignature.Init, 
                new MessageText(testString));

            // Run
            byte[] result = serializer.Serialize(messageStructure);

            // Validate
            builder.Verify(f => f.AppendStructureHeader(StructureSignature.Init, 1));
            builder.Verify(f => f.Append(testString));
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(testBytes[0], result[0]);
        }
    }
}
