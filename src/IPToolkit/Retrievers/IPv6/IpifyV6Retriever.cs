namespace IPToolkit.Retrievers.IPv6
{
    /// <summary>
    /// Class for retrieving IPv4 addresses from ipify.
    /// </summary>
    /// <seealso cref="IRetriever" />
    internal class IpifyV6Retriever : PlainTextRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IpifyV6Retriever"/> class.
        /// </summary>
        public IpifyV6Retriever()
            : base("http://api6.ipify.org")
        {
        }
    }
}
