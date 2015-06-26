using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public enum PackStreamType
    {
        Null,
        Bool,

        Double, 
        /// <summary>
        /// 4 bit integer, the value is embedded in the 'marker'
        /// </summary>
        Integer4,

        /// <summary>
        /// 8 bit integer
        /// </summary>
        Integer8,

        /// <summary>
        /// 16 bit integer
        /// </summary>
        Integer16,

        /// <summary>
        /// 32 bit integer
        /// </summary>
        Integer32,

        /// <summary>
        /// 64 bit integer
        /// </summary>
        Integer64,

        Text,
        List,
        Map,
        Structure
    }
}

