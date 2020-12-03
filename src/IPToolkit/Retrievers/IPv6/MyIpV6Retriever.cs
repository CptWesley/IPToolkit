namespace IPToolkit.Retrievers.IPv6
{
    /// <summary>
    /// Class for retrieving IPv6 addresses from my-ip.
    /// </summary>
    /// <seealso cref="IRetriever" />
    internal class MyIpV6Retriever : PlainTextRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyIpV6Retriever"/> class.
        /// </summary>
        public MyIpV6Retriever()
            : base("http://api6.my-ip.io/ip.txt")
        {
        }
    }
}
