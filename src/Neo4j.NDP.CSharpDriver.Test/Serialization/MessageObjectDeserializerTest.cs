using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4j.NDP.CSharpDriver.Serialization;
using Moq;
using System.IO;

namespace Neo4j.NDP.CSharpDriver.Test.Serialization
{
    [TestClass]
    public class MessageObjectDeserializerTest
    {
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
            Stream stream = new FakeStream();
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
        public void DeserializeBoolTest()
        {
            // Initialize
            Stream stream = new FakeStream();
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
        public void DeserializeInt4Test()
        {
            // Initialize
            Stream stream = new FakeStream();
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
            Stream stream = new FakeStream();
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
            Stream stream = new FakeStream();
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
            Stream stream = new FakeStream();
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
            Stream stream = new FakeStream();
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


        public class FakeStream : Stream
        {
            public override bool CanRead
            {
                get { throw new NotImplementedException(); }
            }

            public override bool CanSeek
            {
                get { throw new NotImplementedException(); }
            }

            public override bool CanWrite
            {
                get { throw new NotImplementedException(); }
            }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override long Length
            {
                get { throw new NotImplementedException(); }
            }

            public override long Position
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }
        }

    }
}
