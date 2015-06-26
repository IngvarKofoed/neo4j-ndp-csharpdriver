using System;


namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Converts .NET types to an array bytes and an array of bytes to .NET types.
    /// </summary>
    public interface IBitConverter
    {
        /// <summary>
        /// Converts a byte to bytes.
        /// </summary>
        /// <param name="value">The byte value to convert.</param>
        /// <returns>The specified byte value as an array of bytes.</returns>
        byte[] GetBytes(byte value);

        /// <summary>
        /// Converts a shot (Int16) to bytes.
        /// </summary>
        /// <param name="value">The short (Int16) value to convert.</param>
        /// <returns>The specified short (Int16) value as an array of bytes.</returns>
        byte[] GetBytes(short value);

        /// <summary>
        /// Converts a shot (UInt16) to bytes.
        /// </summary>
        /// <param name="value">The short (UInt16) value to convert.</param>
        /// <returns>The specified short (UInt16) value as an array of bytes.</returns>
        byte[] GetBytes(ushort value);

        /// <summary>
        /// Converts an int (Int32) to bytes.
        /// </summary>
        /// <param name="value">The int (Int32) value to convert.</param>
        /// <returns>The specified int (Int32) value as an array of bytes.</returns>
        byte[] GetBytes(int value);

        /// <summary>
        /// Converts an uint (UInt32) to bytes.
        /// </summary>
        /// <param name="value">The uint (UInt32) value to convert.</param>
        /// <returns>The specified uint (UInt32) value as an array of bytes.</returns>
        byte[] GetBytes(uint value);

        /// <summary>
        /// Converts an int (Int64) to bytes.
        /// </summary>
        /// <param name="value">The int (Int64) value to convert.</param>
        /// <returns>The specified int (Int64) value as an array of bytes.</returns>
        byte[] GetBytes(Int64 value);

        /// <summary>
        /// Converts an int (double) to bytes.
        /// </summary>
        /// <param name="value">The int (double) value to convert.</param>
        /// <returns>The specified int (double) value as an array of bytes.</returns>
        byte[] GetBytes(double value);

        /// <summary>
        /// Converts an string to bytes.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <returns>The specified string value as an array of bytes.</returns>
        byte[] GetBytes(string value);

        /// <summary>
        /// Converts an byte array to a short.
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A short converted from the byte array.</returns>
        short ToInt16(byte[] bytes);

        /// <summary>
        /// Converts an byte array to a int (Int32).
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A int (Int32) converted from the byte array.</returns>
        int ToInt32(byte[] bytes);

        /// <summary>
        /// Converts an byte array to a int (Int64).
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A int (Int64) converted from the byte array.</returns>
        Int64 ToInt64(byte[] bytes);

        /// <summary>
        /// Converts an byte array to a int (double).
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A int (double) converted from the byte array.</returns>
        double ToDouble(byte[] bytes);

        /// <summary>
        /// Converts an byte array of a UTF8 encoded string to a string
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A string converted from the byte array</returns>
        string ToString(byte[] bytes);
    }
}
