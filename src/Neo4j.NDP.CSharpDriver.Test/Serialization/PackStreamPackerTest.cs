using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neo4j.NDP.CSharpDriver.Serialization;
using System;

namespace Neo4j.NDP.CSharpDriver.Test.Serialization
{
    [TestClass]
    public class PackStreamPackerTest
    {
        [TestMethod]
        public void NullTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.AppendNull();
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(0xC0, result[0]);
        }

        [TestMethod]
        public void TrueTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.Append(true);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(0xC3, result[0]);
        }

        [TestMethod]
        public void FalseTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.Append(false);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(0xC2, result[0]);
        }

        [TestMethod]
        public void Int4Test()
        {
            // Initialize
            const int testValue = 16;

            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.Append(testValue);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(0x10, result[0]);
        }

        [TestMethod]
        public void Int8Test()
        {
            // Initialize
            const int testValue = -100;

            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.Append(testValue);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xC8, result[0]);
            Assert.AreEqual(256 + testValue, result[1]);
        }

        [TestMethod]
        public void Int16Test()
        {
            // Initialize
            const int testValue = 1234;

            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.GetBytes((short)testValue)).Returns(new byte[] { 1 });
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.Append(testValue);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xC9, result[0]);
            Assert.AreEqual(1, result[1]);
        }

        [TestMethod]
        public void Int32Test()
        {
            // Initialize
            const int testValue = 70000;

            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.GetBytes(testValue)).Returns(new byte[] { 1 });
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.Append(testValue);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xCA, result[0]);
            Assert.AreEqual(1, result[1]);
        }

        [TestMethod]
        public void Int64Test()
        {
            // Initialize
            const Int64 testValue = 3000000000;

            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.GetBytes(testValue)).Returns(new byte[] { 1 });
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.Append(testValue);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xCB, result[0]);
            Assert.AreEqual(1, result[1]);
        }

        [TestMethod]
        public void DoubleTest()
        {
            // Initialize
            const double testValue = 1.1;

            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.GetBytes(testValue)).Returns(new byte[] { 1 });
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.Append(testValue);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xC1, result[0]);
            Assert.AreEqual(1, result[1]);
        }

        [TestMethod]
        public void Text4Test()
        {
            // Initialize
            const string testString = "Test";

            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.GetBytes(testString)).Returns(new byte[] { 1, 2, 3 });
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.Append(testString);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(0x83, result[0]); // Marker high, lenght low
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(2, result[2]);
            Assert.AreEqual(3, result[3]);
        }

        [TestMethod]
        public void Text8Test()
        {
            // Initialize
            const string testString = "Test";
            const int length = 20;
            byte[] serializedBytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                serializedBytes[i] = (byte)i;
            }

            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.GetBytes(testString)).Returns(serializedBytes);
            bitConverter.Setup(f => f.GetBytes((byte)length)).Returns(new byte[] { (byte)length });
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.Append(testString);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2 + serializedBytes.Length, result.Length);
            Assert.AreEqual(0xD0, result[0]);
            Assert.AreEqual(length, result[1]);
            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(serializedBytes[i], result[2 + i]);
            }
        }

        [TestMethod]
        public void Text16Test()
        {
            // Initialize
            const string testString = "Test";
            const int length = 300;
            byte[] serializedBytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                serializedBytes[i] = (byte)i;
            }

            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.GetBytes(testString)).Returns(serializedBytes);
            bitConverter.Setup(f => f.GetBytes((ushort)length)).Returns(new byte[] { 123 });
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.Append(testString);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2 + serializedBytes.Length, result.Length);
            Assert.AreEqual(0xD1, result[0]);
            Assert.AreEqual(123, result[1]);
            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(serializedBytes[i], result[2 + i]);
            }
        }

        [TestMethod]
        public void Text32Test()
        {
            // Initialize
            const string testString = "Test";
            const int length = 70000;
            byte[] serializedBytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                serializedBytes[i] = (byte)i;
            }

            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.GetBytes(testString)).Returns(serializedBytes);
            bitConverter.Setup(f => f.GetBytes((int)length)).Returns(new byte[] { 123 });
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.Append(testString);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2 + serializedBytes.Length, result.Length);
            Assert.AreEqual(0xD2, result[0]);
            Assert.AreEqual(123, result[1]);
            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(serializedBytes[i], result[2 + i]);
            }
        }

        [TestMethod]
        public void List4Test()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.AppendListHeader(1);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(0x91, result[0]);
        }

        [TestMethod]
        public void List8Test()
        {
            // Initialize
            int length = 100;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((byte)length)).Returns(new byte[] { 0xAB });

            // Run
            packer.AppendListHeader(length);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xD4, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }

        [TestMethod]
        public void List16Test()
        {
            // Initialize
            int length = 300;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((short)length)).Returns(new byte[] { 0xAB });

            // Run
            packer.AppendListHeader(length);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xD5, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }

        [TestMethod]
        public void List32Test()
        {
            // Initialize
            int length = 70000;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((int)length)).Returns(new byte[] { 0xAB });

            // Run
            packer.AppendListHeader(length);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xD6, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }

        [TestMethod]
        public void Map4Test()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.AppendMapHeader(1);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(0xA1, result[0]);
        }

        [TestMethod]
        public void Map8Test()
        {
            // Initialize
            int length = 100;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((byte)length)).Returns(new byte[] { 0xAB });

            // Run
            packer.AppendMapHeader(length);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xD8, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }

        [TestMethod]
        public void Map16Test()
        {
            // Initialize
            int length = 300;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((short)length)).Returns(new byte[] { 0xAB });

            // Run
            packer.AppendMapHeader(length);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xD9, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }

        [TestMethod]
        public void Map32Test()
        {
            // Initialize
            int length = 70000;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((int)length)).Returns(new byte[] { 0xAB });

            // Run
            packer.AppendMapHeader(length);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xDA, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }

        [TestMethod]
        public void Structure4Test()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.AppendStructureHeader(StructureSignature.Init, 1);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xB1, result[0]);
            Assert.AreEqual((byte)StructureSignature.Init, result[1]);
            
        }

        [TestMethod]
        public void Structure8Test()
        {
            // Initialize
            int length = 100;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((byte)length)).Returns(new byte[] { 0xAB });

            // Run
            packer.AppendStructureHeader(StructureSignature.Init, length);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(0xDC, result[0]);
            Assert.AreEqual(0xAB, result[1]);
            Assert.AreEqual((byte)StructureSignature.Init, result[2]);
        }

        [TestMethod]
        public void Structure16BitTest()
        {
            // Initialize
            int length = 300;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((short)length)).Returns(new byte[] { 0xAB });

            // Run
            packer.AppendStructureHeader(StructureSignature.Init, length);
            byte[] result = packer.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(0xDD, result[0]);
            Assert.AreEqual(0xAB, result[1]);
            Assert.AreEqual((byte)StructureSignature.Init, result[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Structure32Test()
        {
            // Initialize
            int length = 70000;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamPacker packer = new PackStreamPacker(bitConverter.Object);

            // Run
            packer.AppendStructureHeader(StructureSignature.Init, length);
        }
    }
}
