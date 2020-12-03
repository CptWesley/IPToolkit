using System.Net.Http;
using System.Threading.Tasks;

namespace IPToolkit
{
    /// <summary>
    /// Provides extension methods for the <see cref="HttpClient"/> class.
    /// </summary>
    internal static class HttpClientExtensions
    {
        /// <summary>
        /// Send a GET request to the specified Uri as a synchronous operation.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="requestUri">The URI to which the request is send.</param>
        /// <returns>The resulting HTTP response.</returns>
        public static HttpResponseMessage Get(this HttpClient client, string requestUri)
            => Task.Run(() => client.GetAsync(requestUri)).GetAwaiter().GetResult();
    }
}
