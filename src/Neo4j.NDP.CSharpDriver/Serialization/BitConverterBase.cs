﻿using System;
using System.Text;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Base class for <see cref="IBitConverter"/> that handles
    /// little vs big endian.
    /// </summary>
    public abstract class BitConverterBase : IBitConverter
    {
        /// <summary>
        /// Converts a byte to bytes.
        /// </summary>
        /// <param name="value">The byte value to convert.</param>
        /// <returns>The specified byte value as an array of bytes.</returns>
        public byte[] GetBytes(byte value)
        {
            byte[] bytes = new byte[] { value };
            return bytes;
        }

        /// <summary>
        /// Converts a shot (Int16) to bytes.
        /// </summary>
        /// <param name="value">The short (Int16) value to convert.</param>
        /// <returns>The specified short (Int16) value as an array of bytes.</returns>
        public byte[] GetBytes(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            return ToTargetEndian(bytes);
        }

        /// <summary>
        /// Converts a shot (UInt16) to bytes.
        /// </summary>
        /// <param name="value">The short (UInt16) value to convert.</param>
        /// <returns>The specified short (UInt16) value as an array of bytes.</returns>
        public byte[] GetBytes(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            return ToTargetEndian(bytes);
        }

        /// <summary>
        /// Converts an int (Int32) to bytes.
        /// </summary>
        /// <param name="value">The int (Int32) value to convert.</param>
        /// <returns>The specified int (Int32) value as an array of bytes.</returns>
        public byte[] GetBytes(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            return ToTargetEndian(bytes);
        }

        /// <summary>
        /// Converts an uint (UInt32) to bytes.
        /// </summary>
        /// <param name="value">The uint (UInt32) value to convert.</param>
        /// <returns>The specified uint (UInt32) value as an array of bytes.</returns>
        public byte[] GetBytes(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            return ToTargetEndian(bytes);
        }

        /// <summary>
        /// Converts an int (Int64) to bytes.
        /// </summary>
        /// <param name="value">The int (Int64) value to convert.</param>
        /// <returns>The specified int (Int64) value as an array of bytes.</returns>
        public byte[] GetBytes(Int64 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            return ToTargetEndian(bytes);
        }

        /// <summary>
        /// Converts an int (double) to bytes.
        /// </summary>
        /// <param name="value">The int (double) value to convert.</param>
        /// <returns>The specified int (double) value as an array of bytes.</returns>
        public byte[] GetBytes(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            return ToTargetEndian(bytes);
        }

        /// <summary>
        /// Converts an string to bytes.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <returns>The specified string value as an array of bytes.</returns>
        public byte[] GetBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// Converts an byte array to a short.
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A short converted from the byte array.</returns>
        public short ToInt16(byte[] bytes)
        {
            bytes = ToPlatformEndian(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }

        /// <summary>
        /// Converts an byte array to a int (Int32).
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A int (Int32) converted from the byte array.</returns>
        public int ToInt32(byte[] bytes)
        {
            bytes = ToPlatformEndian(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Converts an byte array to a int (Int64).
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A int (Int64) converted from the byte array.</returns>
        public Int64 ToInt64(byte[] bytes)
        {
            bytes = ToPlatformEndian(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// Converts an byte array to a int (double).
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A int (double) converted from the byte array.</returns>
        public double ToDouble(byte[] bytes)
        {
            bytes = ToPlatformEndian(bytes);
            return BitConverter.ToDouble(bytes, 0);
        }

        /// <summary>
        /// Converts an byte array of a UTF8 encoded string to a string
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A string converted from the byte array</returns>
        public string ToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Converts the bytes to the target endian type.
        /// </summary>
        /// <param name="bytes">The bytes to convert.</param>
        /// <returns>The bytes converted to the targert endian type.</returns>
        protected abstract byte[] ToTargetEndian(byte[] bytes);

        /// <summary>
        /// Converts the bytes to the platform endian type.
        /// </summary>
        /// <param name="bytes">The bytes to convert.</param>
        /// <returns>The bytes converted to the platform endian type.</returns>
        protected abstract byte[] ToPlatformEndian(byte[] bytes);
    }
}
