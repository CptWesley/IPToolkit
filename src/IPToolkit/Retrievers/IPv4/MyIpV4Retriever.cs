namespace IPToolkit.Retrievers.IPv4
{
    /// <summary>
    /// Class for retrieving IPv4 addresses from my-ip.
    /// </summary>
    /// <seealso cref="IRetriever" />
    internal class MyIpV4Retriever : PlainTextRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyIpV4Retriever"/> class.
        /// </summary>
        public MyIpV4Retriever()
            : base("http://api4.my-ip.io/ip.txt")
        {
        }
    }
}
