namespace IPToolkit.Retrievers.IPv4
{
    /// <summary>
    /// Class for retrieving IPv4 addresses from whatismyipaddress.
    /// </summary>
    /// <seealso cref="IRetriever" />
    internal class WhatIsMyIpAddressV4Retriever : PlainTextRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhatIsMyIpAddressV4Retriever"/> class.
        /// </summary>
        public WhatIsMyIpAddressV4Retriever()
            : base("http://ipv4bot.whatismyipaddress.com/")
        {
        }
    }
}
