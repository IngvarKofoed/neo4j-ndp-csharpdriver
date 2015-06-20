
namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Types of <see cref="IMessageObject"/>
    /// </summary>
    public enum MessageObjectType
    {
        Null = 0,
        Bool = 1,
        Text = 2,
        Map = 3,
        List = 4,
        Structure = 5
    }
}
