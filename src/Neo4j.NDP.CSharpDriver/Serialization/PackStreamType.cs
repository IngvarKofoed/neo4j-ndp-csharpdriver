using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    public enum PackStreamType
    {
        Null = 0,
        Bool = 1,
        Text = 2,
        List = 3,
        Map = 4,
        Structure = 5
    }
}

