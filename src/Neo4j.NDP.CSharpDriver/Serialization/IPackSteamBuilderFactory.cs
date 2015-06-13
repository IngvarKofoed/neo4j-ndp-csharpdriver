
namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// A factory for creating <see cref="IPackStreamBuilder"/> instances.
    /// </summary>
    public interface IPackSteamBuilderFactory
    {
        /// <summary>
        /// Returns a newly created <see cref="IPackStreamBuilder"/> instance.
        /// </summary>
        /// <returns>A newly created <see cref="IPackStreamBuilder"/> instance.</returns>
        IPackStreamBuilder Create();
    }
}
