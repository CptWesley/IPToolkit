namespace IPToolkit.Retrievers
{
    /// <summary>
    /// Class for retrieving IPv6 addresses from icanhazip.
    /// </summary>
    /// <seealso cref="IRetriever" />
    internal class IcanhazipV6Retriever : PlainTextRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IcanhazipV6Retriever"/> class.
        /// </summary>
        public IcanhazipV6Retriever()
            : base("http://ipv6.icanhazip.com/")
        {
        }
    }
}
