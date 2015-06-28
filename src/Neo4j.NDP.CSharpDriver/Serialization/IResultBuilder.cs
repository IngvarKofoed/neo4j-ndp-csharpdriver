using System;

namespace Neo4j.NDP.CSharpDriver.Serialization
{
    /// <summary>
    /// Builds results of type <typeparamref name="T" /> from records items in the form of <see cref="IMessageList"/>
    /// </summary>
    public interface IResultBuilder<T>
    {
        /// <summary>
        /// Builds results of type <typeparamref name="T" /> from records items in the form of <see cref="IMessageList"/>
        /// </summary>
        /// <param name="recordItems">Record items to build the result of.</param>
        /// <typeparam name="T">
        /// The type to construct through its constructor with the values
        /// of <paramref name="recordItems"/> as arguments to the constructor.
        /// </typeparam>
        T Build(IMessageList recordItems);
    }
}

