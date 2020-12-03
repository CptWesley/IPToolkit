using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        {
            foreach (IRetriever retriever in IPv4Retrievers)
            {
                (bool success, string result) = TryGet(retriever);
                if (success)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the public IPv4 address asynchronously.
        /// </summary>
        /// <returns>Public IPv4 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static async Task<string?> GetPublicIPv4Async()
        {
            List<Task<(bool, string)>> tasks = new List<Task<(bool, string)>>();

            foreach (IRetriever retriever in IPv4Retrievers)
            {
                tasks.Add(TryGetAsync(retriever));
            }

            while (tasks.Any())
            {
                Task<(bool, string)> completed = await Task.WhenAny(tasks).ConfigureAwait(false);
                (bool success, string result) = completed.Result;

                if (success)
                {
                    return result;
                }

                tasks.Remove(completed);
            }

            return null;
        }

        /// <summary>
        /// Gets the public IPv6 address synchronously.
        /// </summary>
        /// <returns>Public IPv6 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static string? GetPublicIPv6()
        {
            foreach (IRetriever retriever in IPv6Retrievers)
            {
                (bool success, string result) = TryGet(retriever);
                if (success)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the public IPv6 address asynchronously.
        /// </summary>
        /// <returns>Public IPv6 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static async Task<string?> GetPublicIPv6Async()
        {
            List<Task<(bool, string)>> tasks = new List<Task<(bool, string)>>();

            foreach (IRetriever retriever in IPv6Retrievers)
            {
                tasks.Add(TryGetAsync(retriever));
            }

            while (tasks.Any())
            {
                Task<(bool, string)> completed = await Task.WhenAny(tasks).ConfigureAwait(false);
                (bool success, string result) = completed.Result;

                if (success)
                {
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
        private static async Task<(bool Success, string Result)> TryGetAsync(IRetriever retriever)
        {
            try
            {
                HttpResponseMessage response = await Client.GetAsync(retriever.Uri).ConfigureAwait(false);
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
