using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neo4j.NDP.CSharpDriver.Serialization;

namespace Neo4j.NDP.CSharpDriver.Test.Serialization
{
    [TestClass]
    public class PackStreamBuilderTest
    {
        [TestMethod]
        public void Text4BitTest()
        {
            // Initialize
            const string testString = "Test";

            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            bitConverter.Setup(f => f.GetBytes(testString)).Returns(new byte[] { 1, 2, 3 });
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);

            // Run
            builder.Append(testString);
            byte[] result = builder.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(0x83, result[0]); // Marker high, lenght low
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(2, result[2]);
            Assert.AreEqual(3, result[3]);
        }

        [TestMethod]
        public void Text8BitTest()
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
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);

            // Run
            builder.Append(testString);
            byte[] result = builder.GetBytes();

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
        public void Text16BitTest()
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
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);

            // Run
            builder.Append(testString);
            byte[] result = builder.GetBytes();

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
        public void Text32BitTest()
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
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);

            // Run
            builder.Append(testString);
            byte[] result = builder.GetBytes();

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
        public void List4BitTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);

            // Run
            builder.AppendListHeader(1);
            byte[] result = builder.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(0x91, result[0]);
        }

        [TestMethod]
        public void List8BitTest()
        {
            // Initialize
            int length = 100;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((byte)length)).Returns(new byte[] { 0xAB });

            // Run
            builder.AppendListHeader(length);
            byte[] result = builder.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xD4, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }

        [TestMethod]
        public void List16BitTest()
        {
            // Initialize
            int length = 300;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((short)length)).Returns(new byte[] { 0xAB });

            // Run
            builder.AppendListHeader(length);
            byte[] result = builder.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xD5, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }

        [TestMethod]
        public void List32BitTest()
        {
            // Initialize
            int length = 70000;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((int)length)).Returns(new byte[] { 0xAB });

            // Run
            builder.AppendListHeader(length);
            byte[] result = builder.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xD6, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }

        [TestMethod]
        public void Map4BitTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);

            // Run
            builder.AppendMapHeader(1);
            byte[] result = builder.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(0xA1, result[0]);
        }

        [TestMethod]
        public void Map8BitTest()
        {
            // Initialize
            int length = 100;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((byte)length)).Returns(new byte[] { 0xAB });

            // Run
            builder.AppendMapHeader(length);
            byte[] result = builder.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xD8, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }

        [TestMethod]
        public void Map16BitTest()
        {
            // Initialize
            int length = 300;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((short)length)).Returns(new byte[] { 0xAB });

            // Run
            builder.AppendMapHeader(length);
            byte[] result = builder.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xD9, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }

        [TestMethod]
        public void Map32BitTest()
        {
            // Initialize
            int length = 70000;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((int)length)).Returns(new byte[] { 0xAB });

            // Run
            builder.AppendMapHeader(length);
            byte[] result = builder.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xDA, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }



        [TestMethod]
        public void Structure4BitTest()
        {
            // Initialize
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);

            // Run
            builder.AppendStructureHeader(StructureSignature.Init, 1);
            byte[] result = builder.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xB1, result[0]);
            Assert.AreEqual((byte)StructureSignature.Init, result[1]);
            
        }

        [TestMethod]
        public void Structure8BitTest()
        {
            // Initialize
            int length = 100;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((byte)length)).Returns(new byte[] { 0xAB });

            // Run
            builder.AppendStructureHeader(StructureSignature.Init, length);
            byte[] result = builder.GetBytes();

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
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((short)length)).Returns(new byte[] { 0xAB });

            // Run
            builder.AppendStructureHeader(StructureSignature.Init, length);
            byte[] result = builder.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(0xDD, result[0]);
            Assert.AreEqual(0xAB, result[1]);
            Assert.AreEqual((byte)StructureSignature.Init, result[2]);
        }

        [TestMethod]
        public void Structure32BitTest()
        {
            // Initialize
            int length = 70000;
            Mock<IBitConverter> bitConverter = new Mock<IBitConverter>();
            PackStreamBuilder builder = new PackStreamBuilder(bitConverter.Object);
            bitConverter.Setup(f => f.GetBytes((int)length)).Returns(new byte[] { 0xAB });

            // Run
            builder.AppendMapHeader(length);
            byte[] result = builder.GetBytes();

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0xDA, result[0]);
            Assert.AreEqual(0xAB, result[1]);
        }
    }
}
