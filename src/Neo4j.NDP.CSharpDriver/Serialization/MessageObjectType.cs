
namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Types of <see cref="IMessageObject"/>
    /// </summary>
    public enum MessageObjectType
    {
        Null = 0,
        Bool = 1,
        Int = 2,
        Text = 3,
        Map = 4,
        List = 5,
        Structure = 6
    }
}
