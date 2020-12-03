namespace IPToolkit.Retrievers
{
    /// <summary>
    /// Abstract retriever for retriever replying with plain text solutions that dont require any parsing.
    /// </summary>
    /// <seealso cref="IRetriever" />
    public abstract class PlainTextRetriever : IRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlainTextRetriever"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public PlainTextRetriever(string uri)
            => Uri = uri;

        /// <inheritdoc/>
        public string Uri { get; }

        /// <inheritdoc/>
        public string Parse(string content)
            => content;
    }
}
