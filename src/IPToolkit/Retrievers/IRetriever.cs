namespace IPToolkit.Retrievers
{
    /// <summary>
    /// Interface for IP retrievers.
    /// </summary>
    internal interface IRetriever
    {
        /// <summary>
        /// Gets the request URI.
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// Parses the http content to the actual IP string.
        /// </summary>
        /// <param name="content">The HTTP response content.</param>
        /// <returns>The found IP.</returns>
        public string Parse(string content);
    }
}
