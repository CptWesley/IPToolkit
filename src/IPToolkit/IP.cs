using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using IPToolkit.Retrievers;
using IPToolkit.Retrievers.IPv4;
using IPToolkit.Retrievers.IPv6;

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
            new IpifyV4Retriever(),
            new WhatIsMyIpAddressV4Retriever(),
            new IcanhazipV4Retriever(),
            new DynDnsRetriever(),
            new MyIpV4Retriever(),
        };

        private static readonly IRetriever[] IPv6Retrievers = new IRetriever[]
        {
            new IpifyV6Retriever(),
            new WhatIsMyIpAddressV6Retriever(),
            new IcanhazipV6Retriever(),
            new MyIpV6Retriever(),
        };

        private static readonly string[] IPv4LocalRetriever = new string[]
        {
            "8.8.8.8",
            "8.8.4.4",
            "1.1.1.1",
            "1.0.0.1",
        };

        private static readonly string[] IPv6LocalRetriever = new string[]
        {
            "2001:4860:4860::8888",
            "2001:4860:4860::8844",
            "2606:4700:4700::1111",
            "2606:4700:4700::1001",
        };

        /// <summary>
        /// Gets the public IPv4 address synchronously.
        /// </summary>
        /// <returns>Public IPv4 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static string? GetPublicIPv4()
            => Get(IPv4Retrievers, TryGetPublic);

        /// <summary>
        /// Gets the public IPv4 address asynchronously.
        /// </summary>
        /// <returns>Public IPv4 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static Task<string?> GetPublicIPv4Async()
            => GetAsync(IPv4Retrievers, TryGetPublicAsync);

        /// <summary>
        /// Gets the public IPv6 address synchronously.
        /// </summary>
        /// <returns>Public IPv6 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static string? GetPublicIPv6()
            => Get(IPv6Retrievers, TryGetPublic);

        /// <summary>
        /// Gets the public IPv6 address asynchronously.
        /// </summary>
        /// <returns>Public IPv6 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static Task<string?> GetPublicIPv6Async()
            => GetAsync(IPv6Retrievers, TryGetPublicAsync);

        /// <summary>
        /// Gets the local IPv4 address synchronously.
        /// </summary>
        /// <returns>Local IPv4 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static string? GetLocalIPv4()
            => Get(IPv4LocalRetriever, TryGetLocalV4);

        /// <summary>
        /// Gets the local IPv4 address asynchronously.
        /// </summary>
        /// <returns>Local IPv4 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static Task<string?> GetLocalIPv4Async()
            => GetAsync(IPv4LocalRetriever, TryGetLocalV4Async);

        /// <summary>
        /// Gets the local IPv6 address synchronously.
        /// </summary>
        /// <returns>Local IPv6 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static string? GetLocalIPv6()
            => Get(IPv6LocalRetriever, TryGetLocalV6);

        /// <summary>
        /// Gets the local IPv6 address asynchronously.
        /// </summary>
        /// <returns>Local IPv6 address if it can be found. <c>null</c> if it can't be determined.</returns>
        public static Task<string?> GetLocalIPv6Async()
            => GetAsync(IPv6LocalRetriever, TryGetLocalV6Async);

        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "We actually want to catch any error.")]
        private static (bool Success, string Result) TryGetPublic(IRetriever retriever)
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
        private static async Task<(bool Success, string Result)> TryGetPublicAsync(IRetriever retriever, CancellationToken cancellationToken)
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

        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "We actually want to catch any error.")]
        private static (bool Success, string Result) TryGetLocalV4(string host)
        {
            try
            {
                using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
                socket.Connect(host, 65530);
                IPEndPoint endPoint = (IPEndPoint)socket.LocalEndPoint;
                return (true, endPoint.Address.ToString());
            }
            catch
            {
                return (false, null!);
            }
        }

        private static Task<(bool Success, string Result)> TryGetLocalV4Async(string host, CancellationToken cancellationToken)
            => Task.Run(() => TryGetLocalV4(host), cancellationToken);

        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "We actually want to catch any error.")]
        private static (bool Success, string Result) TryGetLocalV6(string host)
        {
            try
            {
                using Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, 0);
                socket.Connect(host, 65530);
                IPEndPoint endPoint = (IPEndPoint)socket.LocalEndPoint;
                return (true, endPoint.Address.ToString());
            }
            catch
            {
                return (false, null!);
            }
        }

        private static Task<(bool Success, string Result)> TryGetLocalV6Async(string host, CancellationToken cancellationToken)
            => Task.Run(() => TryGetLocalV6(host), cancellationToken);

        private static async Task<string?> GetAsync<T>(T[] retrievers, Func<T, CancellationToken, Task<(bool Success, string Result)>> func)
        {
            List<Task<(bool, string)>> tasks = new List<Task<(bool, string)>>();
            using CancellationTokenSource tokenSource = new CancellationTokenSource();

            foreach (T retriever in retrievers)
            {
                tasks.Add(func(retriever, tokenSource.Token));
            }

            while (tasks.Any())
            {
                Task<(bool, string)> completed = await Task.WhenAny(tasks).ConfigureAwait(false);
                (bool success, string result) = completed.Result;

                if (success)
                {
                    tokenSource.Cancel();
                    return result.Trim();
                }

                tasks.Remove(completed);
            }

            return null;
        }

        private static string? Get<T>(T[] retrievers, Func<T, (bool Success, string Result)> func)
        {
            foreach (T retriever in retrievers)
            {
                (bool success, string result) = func(retriever);
                if (success)
                {
                    return result.Trim();
                }
            }

            return null;
        }
    }
}
