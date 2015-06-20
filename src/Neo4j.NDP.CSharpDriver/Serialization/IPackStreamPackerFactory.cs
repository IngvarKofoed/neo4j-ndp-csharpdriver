
namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// A factory for creating <see cref="IPackStreamPacker"/> instances.
    /// </summary>
    public interface IPackStreamPackerFactory
    {
        /// <summary>
        /// Returns a newly created <see cref="IPackStreamPacker"/> instance.
        /// </summary>
        /// <returns>A newly created <see cref="IPackStreamPacker"/> instance.</returns>
        IPackStreamPacker Create();
    }
}
