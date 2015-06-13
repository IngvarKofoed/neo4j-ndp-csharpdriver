
namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Factory that creates <see cref="PackStreamBuilder"/>.
    /// </summary>
    public class PackSteamBuilderFactory : IPackSteamBuilderFactory
    {
        private readonly IBitConverter bitConverter;

        /// <summary>
        /// Instantiates an <see cref="PackSteamBuilderFactory"/> instance.
        /// </summary>
        /// <param name="bitConverter">The <see cref="IBitConverter"/> that is going to be used by <see cref="PackStreamBuilder"/>.</param>
        public PackSteamBuilderFactory(IBitConverter bitConverter)
        {
            this.bitConverter = bitConverter;
        }

        public IPackStreamBuilder Create()
        {
            return new PackStreamBuilder(bitConverter);
        }
    }
}
