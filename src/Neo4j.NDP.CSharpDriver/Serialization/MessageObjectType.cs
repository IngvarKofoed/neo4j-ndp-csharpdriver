
namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Types of <see cref="IMessageObject"/>
    /// </summary>
    public enum MessageObjectType
    {
        Null = 0,
        Bool = 1,
        Double = 2,
        Int = 3,
        Text = 4,
        Map = 5,
        List = 6,
        Structure = 7
    }
}
