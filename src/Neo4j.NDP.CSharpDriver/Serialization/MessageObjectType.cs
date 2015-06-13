
namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Types of <see cref="IMessageObject"/>
    /// </summary>
    public enum MessageObjectType
    {
        Null = 0,
        Text = 1,
        Map = 2,
        List = 3,
        Structure = 4
    }
}
