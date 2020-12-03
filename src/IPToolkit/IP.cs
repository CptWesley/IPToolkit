using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IPToolkit.Retrievers;

namespace IPToolkit
{
    /// <summary>
    /// Contains logic for requesting IPs.
    /// </summary>
    public static class IP
    {
        private static readonly HttpClient Client = new HttpClient();
        private static readonly IRetriever[] IPv4Retrievers = new IRetriever[]
        {
            new WtfIsMyIpRetriever(),
            new AwsRetriever(),
            new IcanhazipV4Retriever(),
            new DynDnsRetriever(),
        };

        private static readonly IRetriever[] IPv6Retrievers = new IRetriever[]
        {
            new IcanhazipV6Retriever(),
        };

        /// <summary>
        /// Gets the public IPv4 address synchronously.
        /// </summary>
        /// <returns>Public IPv4 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static string? GetPublicIPv4()
            => GetIP(IPv4Retrievers);

        /// <summary>
        /// Gets the public IPv4 address asynchronously.
        /// </summary>
        /// <returns>Public IPv4 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static Task<string?> GetPublicIPv4Async()
            => GetIPAsync(IPv4Retrievers);

        /// <summary>
        /// Gets the public IPv6 address synchronously.
        /// </summary>
        /// <returns>Public IPv6 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static string? GetPublicIPv6()
            => GetIP(IPv6Retrievers);

        /// <summary>
        /// Gets the public IPv6 address asynchronously.
        /// </summary>
        /// <returns>Public IPv6 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static Task<string?> GetPublicIPv6Async()
            => GetIPAsync(IPv6Retrievers);

        private static string? GetIP(IRetriever[] retrievers)
        {
            foreach (IRetriever retriever in retrievers)
            {
                (bool success, string result) = TryGet(retriever);
                if (success)
                {
                    return result;
                }
            }

            return null;
        }

        private static async Task<string?> GetIPAsync(IRetriever[] retrievers)
        {
            List<Task<(bool, string)>> tasks = new List<Task<(bool, string)>>();
            using CancellationTokenSource tokenSource = new CancellationTokenSource();

            foreach (IRetriever retriever in retrievers)
            {
                tasks.Add(TryGetAsync(retriever, tokenSource.Token));
            }

            while (tasks.Any())
            {
                Task<(bool, string)> completed = await Task.WhenAny(tasks).ConfigureAwait(false);
                (bool success, string result) = completed.Result;

                if (success)
                {
                    tokenSource.Cancel();
                    return result;
                }

                tasks.Remove(completed);
            }

            return null;
        }

        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "We actually want to catch any error.")]
        private static (bool Success, string Result) TryGet(IRetriever retriever)
        {
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(retriever.Uri);
                using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using Stream stream = response.GetResponseStream();
                using StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();

                return (true, retriever.Parse(content));
            }
            catch
            {
                return (false, null!);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "We actually want to catch any error.")]
        private static async Task<(bool Success, string Result)> TryGetAsync(IRetriever retriever, CancellationToken cancellationToken)
        {
            try
            {
                HttpResponseMessage response = await Client.GetAsync(retriever.Uri, cancellationToken).ConfigureAwait(false);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return (true, retriever.Parse(content));
            }
            catch
            {
                return (false, null!);
            }
        }
    }
}
