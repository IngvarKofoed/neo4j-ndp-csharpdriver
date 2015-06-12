using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4j.NDP.CSharpDriver.Serialization;

namespace Neo4j.NDP.CSharpDriver.Test.Serialization
{
    [TestClass]
    public class LittleEndianTargetBitConverterTest
    {
        [TestMethod]
        public void ConvertingFromByteTest()
        {
            var converter = new LittleEndianTargetBitConverter();

            byte value = 0x42;
            byte[] result = converter.GetBytes(value);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(value, result[0]);
        }

        [TestMethod]
        public void ConvertingFromShortTest()
        {
            var converter = new LittleEndianTargetBitConverter();

            short value = 0x4227;
            byte[] result = converter.GetBytes(value);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0x27, result[0]);
            Assert.AreEqual(0x42, result[1]);
        }

        [TestMethod]
        public void ConvertingFromUShortTest()
        {
            var converter = new LittleEndianTargetBitConverter();

            ushort value = 0x4227;
            byte[] result = converter.GetBytes(value);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0x27, result[0]);
            Assert.AreEqual(0x42, result[1]);
        }

        [TestMethod]
        public void ConvertingFromIntTest()
        {
            var converter = new LittleEndianTargetBitConverter();

            int value = 0x42271234;
            byte[] result = converter.GetBytes(value);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(0x34, result[0]);
            Assert.AreEqual(0x12, result[1]);
            Assert.AreEqual(0x27, result[2]);
            Assert.AreEqual(0x42, result[3]);
        }

        [TestMethod]
        public void ConvertingFromUIntTest()
        {
            var converter = new LittleEndianTargetBitConverter();

            uint value = 0x42271234;
            byte[] result = converter.GetBytes(value);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(0x34, result[0]);
            Assert.AreEqual(0x12, result[1]);
            Assert.AreEqual(0x27, result[2]);
            Assert.AreEqual(0x42, result[3]);
        }

        [TestMethod]
        public void ConvertingToShortTest()
        {
            var converter = new LittleEndianTargetBitConverter();

            byte[] value = { 0x42, 0x27 };
            int result = converter.ToInt16(value);
            Assert.AreEqual(10050, result);
        }

        [TestMethod]
        public void ConvertingToIntTest()
        {
            var converter = new LittleEndianTargetBitConverter();

            byte[] value = { 0x42, 0x27, 0x12, 0x34 };
            int result = converter.ToInt32(value);
            Assert.AreEqual(873604930, result);
        }

        [TestMethod]
        public void ConvertingToStringTest()
        {
            var converter = new LittleEndianTargetBitConverter();

            string originalString = "aBc-æøåÅøæ123";
            byte[] value = { 0x61, 0x42, 0x63, 0x2D, 0xC3, 0xA6, 0xC3, 0xB8, 0xC3, 0xA5, 0xC3, 0x85, 0xC3, 0xB8, 0xC3, 0xA6, 0x31, 0x32, 0x33 }; // "aBc-æøåÅøæ123"
            string result = converter.ToString(value);

            Assert.AreEqual(originalString, result);
        }
    }
}
