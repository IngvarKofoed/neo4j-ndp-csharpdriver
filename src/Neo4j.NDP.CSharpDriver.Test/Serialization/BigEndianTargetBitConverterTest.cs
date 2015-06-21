using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4j.NDP.CSharpDriver.Serialization;
using System;

namespace Neo4j.NDP.CSharpDriver.Test.Serialization
{
    [TestClass]
    public class BigEndianTargetBitConverterTest
    {
        [TestMethod]
        public void ConvertingFromByteTest()
        {
            var converter = new BigEndianTargetBitConverter();

            byte value = 0x42;
            byte[] result = converter.GetBytes(value);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(value, result[0]);
        }

        [TestMethod]
        public void ConvertingFromShortTest()
        {
            var converter = new BigEndianTargetBitConverter();

            short value = 0x4227;
            byte[] result = converter.GetBytes(value);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0x42, result[0]);
            Assert.AreEqual(0x27, result[1]);
        }

        [TestMethod]
        public void ConvertingFromUShortTest()
        {
            var converter = new BigEndianTargetBitConverter();

            ushort value = 0x4227;
            byte[] result = converter.GetBytes(value);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(0x42, result[0]);
            Assert.AreEqual(0x27, result[1]);
        }

        [TestMethod]
        public void ConvertingFromIntTest()
        {
            var converter = new BigEndianTargetBitConverter();

            int value = 0x42271234;
            byte[] result = converter.GetBytes(value);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(0x42, result[0]);
            Assert.AreEqual(0x27, result[1]);
            Assert.AreEqual(0x12, result[2]);
            Assert.AreEqual(0x34, result[3]);
        }

        [TestMethod]
        public void ConvertingFromUIntTest()
        {
            var converter = new BigEndianTargetBitConverter();

            uint value = 0x42271234;
            byte[] result = converter.GetBytes(value);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(0x42, result[0]);
            Assert.AreEqual(0x27, result[1]);
            Assert.AreEqual(0x12, result[2]);
            Assert.AreEqual(0x34, result[3]);
        }

        [TestMethod]
        public void ConvertingFromInt64Test()
        {
            var converter = new BigEndianTargetBitConverter();

            Int64 value = 0x4227123498765432;
            byte[] result = converter.GetBytes(value);

            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.Length);
            Assert.AreEqual(0x42, result[0]);
            Assert.AreEqual(0x27, result[1]);
            Assert.AreEqual(0x12, result[2]);
            Assert.AreEqual(0x34, result[3]);
            Assert.AreEqual(0x98, result[4]);
            Assert.AreEqual(0x76, result[5]);
            Assert.AreEqual(0x54, result[6]);
            Assert.AreEqual(0x32, result[7]);
        }

        [TestMethod]
        public void ConvertingFromDoubleTest()
        {
            var converter = new BigEndianTargetBitConverter();

            double value = 1.1;
            byte[] result = converter.GetBytes(value);
             
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.Length);
            Assert.AreEqual(0x3F, result[0]);
            Assert.AreEqual(0xF1, result[1]);
            Assert.AreEqual(0x99, result[2]);
            Assert.AreEqual(0x99, result[3]);
            Assert.AreEqual(0x99, result[4]);
            Assert.AreEqual(0x99, result[5]);
            Assert.AreEqual(0x99, result[6]);
            Assert.AreEqual(0x9A, result[7]);
        }

        [TestMethod]
        public void ConvertingFromStringTest()
        {
            var converter = new BigEndianTargetBitConverter();

            string value = "aBc-æøåÅøæ123";
            byte[] expectedValue = { 0x61, 0x42, 0x63, 0x2D, 0xC3, 0xA6, 0xC3, 0xB8, 0xC3, 0xA5, 0xC3, 0x85, 0xC3, 0xB8, 0xC3, 0xA6, 0x31, 0x32, 0x33 };

            byte[] result = converter.GetBytes(value);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedValue.Length, result.Length);
            for (int i = 0; i < expectedValue.Length; i++)
            {
                Assert.AreEqual(expectedValue[i], result[i]);
            }
        }
        
        [TestMethod]
        public void ConvertingToShortTest()
        {
            var converter = new BigEndianTargetBitConverter();

            byte[] value = { 0x42, 0x27 };
            int result = converter.ToInt16(value);
            Assert.AreEqual(16935, result);
        }

        [TestMethod]
        public void ConvertingToIntTest()
        {
            var converter = new BigEndianTargetBitConverter();

            byte[] value = { 0x42, 0x27, 0x12, 0x34 };
            int result = converter.ToInt32(value);
            Assert.AreEqual(1109856820, result);
        }

        [TestMethod]
        public void ConvertingToDoubleTest()
        {
            var converter = new BigEndianTargetBitConverter();

            byte[] value = { 0xBF, 0xF1, 0x99, 0x99, 0x99, 0x99, 0x99, 0x9A };
            double result = converter.ToDouble(value);
            Assert.IsTrue(Math.Abs(-1.1 - result) < 0.000001);
        }

        [TestMethod]
        public void ConvertingToStringTest()
        {
            var converter = new BigEndianTargetBitConverter();

            string originalString = "aBc-æøåÅøæ123";
            byte[] value = { 0x61, 0x42, 0x63, 0x2D, 0xC3, 0xA6, 0xC3, 0xB8, 0xC3, 0xA5, 0xC3, 0x85, 0xC3, 0xB8, 0xC3, 0xA6, 0x31, 0x32, 0x33 }; // "aBc-æøåÅøæ123"
            string result = converter.ToString(value);

            Assert.AreEqual(originalString, result);
        }
    }
}
