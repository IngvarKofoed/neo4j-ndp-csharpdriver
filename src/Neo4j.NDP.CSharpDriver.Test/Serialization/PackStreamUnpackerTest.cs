﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            byte[] streamBytes = new byte[] { 0xEF };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadNextTypeNullTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            unpacker.ReadNextType(null);
        }

        [TestMethod]
        public void NullResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xC0 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasNoValues(result);
            Assert.AreEqual(PackStreamType.Null, result.Type);
        }

        [TestMethod]
        public void FalseResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xC2 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasBoolValues(result);
            Assert.AreEqual(PackStreamType.Bool, result.Type);
            Assert.AreEqual(false, result.BoolValue.Value);
        }

        [TestMethod]
        public void TrueResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xC3 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasBoolValues(result);
            Assert.AreEqual(PackStreamType.Bool, result.Type);
            Assert.AreEqual(true, result.BoolValue.Value);
        }

        [TestMethod]
        public void DoubleResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xC1 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasNoValues(result);
            Assert.AreEqual(PackStreamType.Double, result.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadDoubleNullTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            unpacker.ReadDouble(null);
        }

        [TestMethod]
        public void ReadDoubleTest()
        {
            // Initialize
            const double testValue = 1.1;
            byte[] streamBytes = new byte[8];
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToDouble(streamBytes)).Returns(testValue);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            double result = GetResult(s => unpacker.ReadDouble(s), streamBytes);

            // Validate
            Assert.AreEqual(testValue, result);
        }

        [TestMethod]
        public void Int4NegativeResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xF0 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Integer4, result.Type);
            Assert.AreEqual(-16, result.IntValue.Value);
        }

        [TestMethod]
        public void Int4PositiveResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0x7F };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Integer4, result.Type);
            Assert.AreEqual(127, result.IntValue.Value);
        }

        [TestMethod]
        public void Int8ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xC8 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Integer8, result.Type);
            Assert.AreEqual(1, result.IntValue.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadInt8NullTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            unpacker.ReadInt8(null);
        }

        [TestMethod]
        public void ReadInt8MinTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { (byte)(256 - 128) };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            int result = GetResult(s => unpacker.ReadInt8(s), streamBytes);

            // Validate
            Assert.AreEqual(-128, result);
        }

        [TestMethod]
        public void ReadInt8MaxTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { (byte)(256 - 17) };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            int result = GetResult(s => unpacker.ReadInt8(s), streamBytes);

            // Validate
            Assert.AreEqual(-17, result);
        }

        [TestMethod]
        public void Int16ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xC9 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Integer16, result.Type);
            Assert.AreEqual(2, result.IntValue.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadInt16NullTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            unpacker.ReadInt16(null);
        }

        [TestMethod]
        public void Read16MinTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0x12, 0x34 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToInt16(It.Is<byte[]>(g => ArraysEqual(g, streamBytes)))).Returns(10);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            int result = GetResult(s => unpacker.ReadInt16(s), streamBytes);

            // Validate
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void Int32ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xCA };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Integer32, result.Type);
            Assert.AreEqual(4, result.IntValue.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadInt32NullTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            unpacker.ReadInt32(null);
        }

        [TestMethod]
        public void Read32MinTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToInt32(It.Is<byte[]>(g => ArraysEqual(g, streamBytes)))).Returns(10);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            int result = GetResult(s => unpacker.ReadInt32(s), streamBytes);

            // Validate
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void Int64ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xCB };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Integer64, result.Type);
            Assert.AreEqual(8, result.IntValue.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadInt64NullTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            unpacker.ReadInt64(null);
        }

        [TestMethod]
        public void Read64MinTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x12, 0x34, 0x56, 0x78 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToInt64(It.Is<byte[]>(g => ArraysEqual(g, streamBytes)))).Returns(10);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            Int64 result = GetResult(s => unpacker.ReadInt64(s), streamBytes);

            // Validate
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void Text4ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0x82 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Text, result.Type);
            Assert.AreEqual(2, result.IntValue);
        }

        [TestMethod]
        public void Text8ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xD0, 0x10 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Text, result.Type);
            Assert.AreEqual(16, result.IntValue);
        }

        [TestMethod]
        public void Text16ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xD1, 0x01, 0x02 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToInt16(It.Is<byte[]>(g => ArraysEqual(g, streamBytes.Skip(1).ToArray())))).Returns(300);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Text, result.Type);
            Assert.AreEqual(300, result.IntValue);
        }

        [TestMethod]
        public void Text32ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xD2, 0x01, 0x02, 0x03, 0x04 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToInt32(It.Is<byte[]>(g => ArraysEqual(g, streamBytes.Skip(1).ToArray())))).Returns(300);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Text, result.Type);
            Assert.AreEqual(300, result.IntValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadTextNullTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            unpacker.ReadText(null, 10);
        }

        [TestMethod]
        public void ReadTextTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0x41, 0x42 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToString(It.Is<byte[]>(g => ArraysEqual(g, streamBytes)))).Returns("Test");
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            string result = GetResult(s => unpacker.ReadText(s, streamBytes.Length), streamBytes);

            // Validate
            Assert.AreEqual("Test", result);
        }

        [TestMethod]
        public void List4ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0x92 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.List, result.Type);
            Assert.AreEqual(2, result.IntValue);
        }

        [TestMethod]
        public void List8ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xD4, 0x10 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.List, result.Type);
            Assert.AreEqual(16, result.IntValue);
        }

        [TestMethod]
        public void List16ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xD5, 0x01, 0x02 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToInt16(It.Is<byte[]>(g => ArraysEqual(g, streamBytes.Skip(1).ToArray())))).Returns(300);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.List, result.Type);
            Assert.AreEqual(300, result.IntValue);
        }

        [TestMethod]
        public void List32ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xD6, 0x01, 0x02, 0x03, 0x04 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToInt32(It.Is<byte[]>(g => ArraysEqual(g, streamBytes.Skip(1).ToArray())))).Returns(300);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.List, result.Type);
            Assert.AreEqual(300, result.IntValue);
        }

        [TestMethod]
        public void Map4ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xA2 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Map, result.Type);
            Assert.AreEqual(2, result.IntValue);
        }

        [TestMethod]
        public void Map8ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xD8, 0x10 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Map, result.Type);
            Assert.AreEqual(16, result.IntValue);
        }

        [TestMethod]
        public void Map16ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xD9, 0x01, 0x02 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToInt16(It.Is<byte[]>(g => ArraysEqual(g, streamBytes.Skip(1).ToArray())))).Returns(300);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Map, result.Type);
            Assert.AreEqual(300, result.IntValue);
        }




        [TestMethod]
        public void Structure4ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xB2 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Structure, result.Type);
            Assert.AreEqual(2, result.IntValue);
        }

        [TestMethod]
        public void Structure8ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xDC, 0x10 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Structure, result.Type);
            Assert.AreEqual(16, result.IntValue);
        }

        [TestMethod]
        public void Structure16ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xDD, 0x01, 0x02 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToInt16(It.Is<byte[]>(g => ArraysEqual(g, streamBytes.Skip(1).ToArray())))).Returns(300);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Structure, result.Type);
            Assert.AreEqual(300, result.IntValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadStructureSignatureNullTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            unpacker.ReadStructureSignature(null);
        }

        [TestMethod]
        public void ReadStructureSignatureTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0x01 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            StructureSignature result = GetResult(s => unpacker.ReadStructureSignature(s), streamBytes);

            // Validate
            Assert.AreEqual(StructureSignature.Init, result);
        }


        [TestMethod]
        public void Map32ResultTest()
        {
            // Initialize
            byte[] streamBytes = new byte[] { 0xDA, 0x01, 0x02, 0x03, 0x04 };
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.ToInt32(It.Is<byte[]>(g => ArraysEqual(g, streamBytes.Skip(1).ToArray())))).Returns(300);
            IPackStreamUnpacker unpacker = new PackStreamUnpacker(bitConverter.Object);

            // Run
            PackStreamUnpackerResult result = GetResult(s => unpacker.ReadNextType(s), streamBytes);

            // Validate
            ValidateHasIntValues(result);
            Assert.AreEqual(PackStreamType.Map, result.Type);
            Assert.AreEqual(300, result.IntValue);
        }
        private void ValidateHasNoValues(PackStreamUnpackerResult result)
        {
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IntValue.HasValue);
            Assert.IsFalse(result.BoolValue.HasValue);
        }

        private void ValidateHasIntValues(PackStreamUnpackerResult result)
        {
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IntValue.HasValue);
            Assert.IsFalse(result.BoolValue.HasValue);
        }

        private void ValidateHasBoolValues(PackStreamUnpackerResult result)
        {
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IntValue.HasValue);
            Assert.IsTrue(result.BoolValue.HasValue);
        }

        /// <summary>
        /// Helper method for wrapping the stream stuff
        /// </summary>
        private T GetResult<T>(Func<Stream, T> action, byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                return action(stream);
            }
        }

        private static bool ArraysEqual<T>(T[] a1, T[] a2)
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
