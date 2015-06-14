
namespace Neo4j.NDP.CSharpDriver
{
    /// <summary>
    /// Structure signatures.
    /// </summary>
    public enum StructureSignature
    {
        /// <summary>
        /// Used when initializing the connection.
        /// </summary>
        Init = 0x01,

        /// <summary>
        /// Used when acknowledging a raised failure on the server.
        /// </summary>
        AcknowledgeFailure = 0x0F,

        // TODO: Write this
        /// <summary>
        /// </summary>
        DiscardAll = 0x2F,

        /// <summary>
        /// Used to pull all records after a run has benn issued.
        /// </summary>
        PullAll = 0x3F,

        /// <summary>
        /// Used to execute a statement.
        /// </summary>
        Run = 0x10,

        /// <summary>
        /// Execution was a success.
        /// </summary>
        Success = 0x70,

        /// <summary>
        /// Structure containing a record.
        /// </summary>
        Record = 0x71,

        /// <summary>
        /// Structure containing a node.
        /// </summary>
        Node = 0x4E,

        // TODO: Write this
        /// <summary>
        /// </summary>
        Ignored = 0x72,

        /// <summary>
        /// Execution was a failure.
        /// </summary>
        Failure = 0x7F,
    }
}
