using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neo4j.NDP.CSharpDriver.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Neo4j.NDP.CSharpDriver.Test.Serialization
{
    [TestClass]
    public class MessageObjectDeserializerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeserializeNullPackStreamUnpackerTest()
        {
            // Run
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeserializeNullByteTest()
        {
            // Initialize
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);
            byte[] data = null;

            // Run
            deserializer.Deserialize(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeserializeNullStreamTest()
        {
            // Initialize
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);
            Stream stream = null;

            // Run
            deserializer.Deserialize(stream);
        }

        [TestMethod]
        public void DeserializeNullTest()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Null));
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            IMessageNull messageNull = deserializer.Deserialize(stream) as IMessageNull;

            // Validate
            Assert.IsNotNull(messageNull);
            Assert.AreEqual(MessageObjectType.Null, messageNull.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeserializeBoolMissingValueTest()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Bool));
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            deserializer.Deserialize(stream);
        }

        [TestMethod]
        public void DeserializeBoolTest()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Bool, true));
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            IMessageBool messageBool = deserializer.Deserialize(stream) as IMessageBool;

            // Validate
            Assert.IsNotNull(messageBool);
            Assert.AreEqual(MessageObjectType.Bool, messageBool.Type);
            Assert.AreEqual(true, messageBool.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeserializeInt4MissingValueTest()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Integer4));
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            deserializer.Deserialize(stream);
        }

        [TestMethod]
        public void DeserializeInt4Test()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Integer4, 2));
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            IMessageInt messageInt = deserializer.Deserialize(stream) as IMessageInt;

            // Validate
            Assert.IsNotNull(messageInt);
            Assert.AreEqual(MessageObjectType.Int, messageInt.Type);
            Assert.AreEqual(2, messageInt.Value);
        }

        [TestMethod]
        public void DeserializeInt8Test()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Integer8));
            unpacker.Setup(f => f.ReadInt8(stream)).Returns(2);
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            IMessageInt messageInt = deserializer.Deserialize(stream) as IMessageInt;

            // Validate
            Assert.IsNotNull(messageInt);
            Assert.AreEqual(MessageObjectType.Int, messageInt.Type);
            Assert.AreEqual(2, messageInt.Value);
        }

        [TestMethod]
        public void DeserializeInt16Test()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Integer16));
            unpacker.Setup(f => f.ReadInt16(stream)).Returns(2);
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            IMessageInt messageInt = deserializer.Deserialize(stream) as IMessageInt;

            // Validate
            Assert.IsNotNull(messageInt);
            Assert.AreEqual(MessageObjectType.Int, messageInt.Type);
            Assert.AreEqual(2, messageInt.Value);
        }

        [TestMethod]
        public void DeserializeInt32Test()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Integer32));
            unpacker.Setup(f => f.ReadInt32(stream)).Returns(2);
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            IMessageInt messageInt = deserializer.Deserialize(stream) as IMessageInt;

            // Validate
            Assert.IsNotNull(messageInt);
            Assert.AreEqual(MessageObjectType.Int, messageInt.Type);
            Assert.AreEqual(2, messageInt.Value);
        }

        [TestMethod]
        public void DeserializeInt64Test()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Integer64));
            unpacker.Setup(f => f.ReadInt64(stream)).Returns(2);
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            IMessageInt messageInt = deserializer.Deserialize(stream) as IMessageInt;

            // Validate
            Assert.IsNotNull(messageInt);
            Assert.AreEqual(MessageObjectType.Int, messageInt.Type);
            Assert.AreEqual(2, messageInt.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeserializeTextMissingValueTest()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Text));
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            deserializer.Deserialize(stream);
        }

        [TestMethod]
        public void DeserializeTextTest()
        {
            // Initialize
            const string testText = "Test";
            const int testTextLength = 4;
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Text, testTextLength));
            unpacker.Setup(f => f.ReadText(stream, testTextLength)).Returns(testText);
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            IMessageText messageText = deserializer.Deserialize(stream) as IMessageText;

            // Validate
            Assert.IsNotNull(messageText);
            Assert.AreEqual(MessageObjectType.Text, messageText.Type);
            Assert.AreEqual(testText, messageText.Text);
        }

        [TestMethod]
        public void DeserializeZeroLengthTextTest()
        {
            // Initialize
            const string testText = "";
            const int testTextLength = 0;
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Text, testTextLength));
            unpacker.Setup(f => f.ReadText(stream, testTextLength)).Returns(testText);
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            IMessageText messageText = deserializer.Deserialize(stream) as IMessageText;

            // Validate
            Assert.IsNotNull(messageText);
            Assert.AreEqual(MessageObjectType.Text, messageText.Type);
            Assert.AreEqual(testText, messageText.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeserializeListMissingValueTest()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.List));
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            deserializer.Deserialize(stream);
        }

        [TestMethod]
        public void DeserializeListTest()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            Queue<PackStreamUnpackerResult> multipleResults = new Queue<PackStreamUnpackerResult>(new []
            {
                new PackStreamUnpackerResult(PackStreamType.List, 1),
                new PackStreamUnpackerResult(PackStreamType.Null)
            });

            unpacker.Setup(f => f.ReadNextType(stream)).Returns(() => multipleResults.Dequeue());
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            IMessageList messageList = deserializer.Deserialize(stream) as IMessageList;

            // Validate
            Assert.IsNotNull(messageList);
            Assert.AreEqual(MessageObjectType.List, messageList.Type);
            Assert.AreEqual(1, messageList.Items.Count);
            Assert.AreEqual(MessageObjectType.Null, messageList.Items[0].Type);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeserializeMapMissingValueTest()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Map));
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            deserializer.Deserialize(stream);
        }

        [TestMethod]
        public void DeserializeMapTest()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            Queue<PackStreamUnpackerResult> multipleResults = new Queue<PackStreamUnpackerResult>(new[]
            {
                new PackStreamUnpackerResult(PackStreamType.Map, 1),
                new PackStreamUnpackerResult(PackStreamType.Bool, true),
                new PackStreamUnpackerResult(PackStreamType.Null)
            });

            unpacker.Setup(f => f.ReadNextType(stream)).Returns(() => multipleResults.Dequeue());
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            IMessageMap messageMap = deserializer.Deserialize(stream) as IMessageMap;

            // Validate
            Assert.IsNotNull(messageMap);
            Assert.AreEqual(MessageObjectType.Map, messageMap.Type);
            Assert.AreEqual(1, messageMap.Map.Count);
            Assert.AreEqual(MessageObjectType.Bool, messageMap.Map.Keys.First().Type);
            Assert.AreEqual(MessageObjectType.Null, messageMap.Map.Values.First().Type);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeserializeStructureMissingValueTest()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(new PackStreamUnpackerResult(PackStreamType.Structure));
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            deserializer.Deserialize(stream);
        }

        [TestMethod]
        public void DeserializeStructureTest()
        {
            // Initialize
            Mock<Stream> streamMock = new Mock<Stream>();
            Stream stream = streamMock.Object;
            Mock<IPackStreamUnpacker> unpacker = new Mock<IPackStreamUnpacker>();
            Queue<PackStreamUnpackerResult> multipleResults = new Queue<PackStreamUnpackerResult>(new[]
            {
                new PackStreamUnpackerResult(PackStreamType.Structure, 1),
                new PackStreamUnpackerResult(PackStreamType.Bool, true)
            });
            unpacker.Setup(f => f.ReadNextType(stream)).Returns(() => multipleResults.Dequeue());
            unpacker.Setup(f => f.ReadStructureSignature(stream)).Returns(StructureSignature.Init);
            IMessageObjectDeserializer deserializer = new MessageObjectDeserializer(unpacker.Object);

            // Run
            IMessageStructure messageStructure = deserializer.Deserialize(stream) as IMessageStructure;

            // Validate
            Assert.IsNotNull(messageStructure);
            Assert.AreEqual(MessageObjectType.Structure, messageStructure.Type);
            Assert.AreEqual(StructureSignature.Init, messageStructure.Signature);
            Assert.AreEqual(1, messageStructure.Fields.Count);
            Assert.AreEqual(MessageObjectType.Bool, messageStructure.Fields[0].Type);
        }
    }
}
