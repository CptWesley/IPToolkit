namespace IPToolkit.Retrievers.IPv4
{
    /// <summary>
    /// Class for retrieving IPv4 addresses from AWS.
    /// </summary>
    /// <seealso cref="IRetriever" />
    internal class AwsRetriever : PlainTextRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwsRetriever"/> class.
        /// </summary>
        public AwsRetriever()
            : base("http://checkip.amazonaws.com/")
        {
        }
    }
}
