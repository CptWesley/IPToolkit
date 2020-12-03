namespace IPToolkit.Retrievers.IPv4
{
    /// <summary>
    /// Class for retrieving IPv4 addresses from wtfismyip.
    /// </summary>
    /// <seealso cref="IRetriever" />
    internal class WtfIsMyIpRetriever : PlainTextRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WtfIsMyIpRetriever"/> class.
        /// </summary>
        public WtfIsMyIpRetriever()
            : base("http://wtfismyip.com/text")
        {
        }
    }
}
