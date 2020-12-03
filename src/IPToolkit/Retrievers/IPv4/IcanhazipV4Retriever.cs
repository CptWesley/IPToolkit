namespace IPToolkit.Retrievers.IPv4
{
    /// <summary>
    /// Class for retrieving IPv4 addresses from icanhazip.
    /// </summary>
    /// <seealso cref="IRetriever" />
    internal class IcanhazipV4Retriever : PlainTextRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IcanhazipV4Retriever"/> class.
        /// </summary>
        public IcanhazipV4Retriever()
            : base("http://ipv4.icanhazip.com/")
        {
        }
    }
}
