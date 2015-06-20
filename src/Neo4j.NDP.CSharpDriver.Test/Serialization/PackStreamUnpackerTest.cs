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
    public class PackStreamUnpackerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullBitConverterTest()
        {
            // Run
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnexpectedDataTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0x00 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result;
            using (MemoryStream stream = new MemoryStream(streamBytes))
            {
                result = unpacker.ReadNextType(stream);
            }
        }

        [TestMethod]
        public void NullResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xC0 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result;
            using (MemoryStream stream = new MemoryStream(streamBytes))
            {
                result = unpacker.ReadNextType(stream);
            }

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(PackStreamType.Null, result.Type);
            Assert.IsFalse(result.Length.HasValue);
        }

        [TestMethod]
        public void Text4BitResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0x82 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result;
            using (MemoryStream stream = new MemoryStream(streamBytes))
            {
                result = unpacker.ReadNextType(stream);
            }

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(PackStreamType.Text, result.Type);
            Assert.IsTrue(result.Length.HasValue);
            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void Text8BitResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xD0, 0x10 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result;
            using (MemoryStream stream = new MemoryStream(streamBytes))
            {
                result = unpacker.ReadNextType(stream);
            }

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(PackStreamType.Text, result.Type);
            Assert.IsTrue(result.Length.HasValue);
            Assert.AreEqual(16, result.Length);
        }

        [TestMethod]
        public void Text16BitResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xD1, 0x01, 0x02 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToInt16(It.Is<byte[]>(g => ArraysEqual(g, streamBytes.Skip(1).ToArray())))).Returns(300);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result;
            using (MemoryStream stream = new MemoryStream(streamBytes))
            {
                result = unpacker.ReadNextType(stream);
            }

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(PackStreamType.Text, result.Type);
            Assert.IsTrue(result.Length.HasValue);
            Assert.AreEqual(300, result.Length);
        }

        [TestMethod]
        public void Text32BitResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xD2, 0x01, 0x02, 0x03, 0x04 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToInt32(It.Is<byte[]>(g => ArraysEqual(g, streamBytes.Skip(1).ToArray())))).Returns(300);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result;
            using (MemoryStream stream = new MemoryStream(streamBytes))
            {
                result = unpacker.ReadNextType(stream);
            }

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(PackStreamType.Text, result.Type);
            Assert.IsTrue(result.Length.HasValue);
            Assert.AreEqual(300, result.Length);
        }


        static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }
    }
}
