
namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Factory that creates <see cref="PackStreamPacker"/>.
    /// </summary>
    public class PackStreamPackerFactory : IPackStreamPackerFactory
    {
        private readonly IBitConverter bitConverter;

        /// <summary>
        /// Instantiates an <see cref="PackSteamPackerFactory"/> instance.
        /// </summary>
        /// <param name="bitConverter">The <see cref="IBitConverter"/> that is going to be used by <see cref="PackStreamPacker"/>.</param>
        public PackStreamPackerFactory(IBitConverter bitConverter)
        {
            this.bitConverter = bitConverter;
        }

        public IPackStreamPacker Create()
        {
            return new PackStreamPacker(bitConverter);
        }
    }
}
