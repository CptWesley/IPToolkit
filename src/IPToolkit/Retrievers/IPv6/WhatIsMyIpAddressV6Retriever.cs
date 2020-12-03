namespace IPToolkit.Retrievers.IPv6
{
    /// <summary>
    /// Class for retrieving IPv6 addresses from whatismyipaddress.
    /// </summary>
    /// <seealso cref="IRetriever" />
    internal class WhatIsMyIpAddressV6Retriever : PlainTextRetriever
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhatIsMyIpAddressV6Retriever"/> class.
        /// </summary>
        public WhatIsMyIpAddressV6Retriever()
            : base("http://ipv6bot.whatismyipaddress.com/")
        {
        }
    }
}
