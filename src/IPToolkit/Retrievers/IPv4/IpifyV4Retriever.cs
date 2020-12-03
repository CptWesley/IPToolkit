namespace IPToolkit.Retrievers.IPv4
{
    /// <summary>
    /// Class for retrieving IPv4 addresses from ipify.
    /// </summary>
    /// <seealso cref="IRetriever" />
    internal class IpifyV4Retriever : PlainTextRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IpifyV4Retriever"/> class.
        /// </summary>
        public IpifyV4Retriever()
            : base("http://api.ipify.org")
        {
        }
    }
}
