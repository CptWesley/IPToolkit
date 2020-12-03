namespace IPToolkit.Retrievers.IPv4
{
    /// <summary>
    /// Class for retrieving IPv4 addresses from dyndns.
    /// </summary>
    /// <seealso cref="IRetriever" />
    internal class DynDnsRetriever : IRetriever
    {
        /// <inheritdoc/>
        public string Uri => "http://checkip.dyndns.com/";

        /// <inheritdoc/>
        public string Parse(string content)
            => content.Replace("<html><head><title>Current IP Check</title></head><body>Current IP Address:", string.Empty).Replace("</body></html>", string.Empty).Trim();
    }
}
