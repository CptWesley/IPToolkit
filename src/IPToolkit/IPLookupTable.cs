using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace IPToolkit
{
    /// <summary>
    /// Class maintaining an IP lookup table.
    /// </summary>
    public class IPLookupTable
    {
        private Dictionary<string, string?> four2six = new Dictionary<string, string?>();
        private Dictionary<string, string?> six2four = new Dictionary<string, string?>();

        /// <summary>
        /// Builds a lookup table for the local network adapters.
        /// </summary>
        /// <returns>The created lookup table.</returns>
        public static IPLookupTable CreateLocal()
        {
            IPLookupTable result = new IPLookupTable();
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

            foreach (IPInterfaceProperties adapter in adapters.Where(x => x.NetworkInterfaceType != NetworkInterfaceType.Loopback).Select(x => x.GetIPProperties()))
            {
                string ipv4 = adapter.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetwork).Address.ToString();
                string ipv6 = adapter.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetworkV6).Address.ToString();

                result.Add(ipv4, ipv6);
            }

            return result;
        }

        /// <summary>
        /// Tries to get the IPv4 address matching the given IPv6 address.
        /// </summary>
        /// <param name="ipv6">The IPv6 address.</param>
        /// <returns>The matched IPv4 address, if it could be found. <c>null</c> otherwise.</returns>
        public string? GetIPv4(string? ipv6)
        {
            if (ipv6 != null && six2four.TryGetValue(ipv6, out string? value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// Tries to get the IPv6 address matching the given IPv4 address.
        /// </summary>
        /// <param name="ipv4">The IPv4 address.</param>
        /// <returns>The matched IPv6 address, if it could be found. <c>null</c> otherwise.</returns>
        public string? GetIPv6(string? ipv4)
        {
            if (ipv4 != null && four2six.TryGetValue(ipv4, out string? value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// Adds the specified IP lookup combination.
        /// </summary>
        /// <param name="ipv4">The IPv4 address..</param>
        /// <param name="ipv6">The IPv6 address.</param>
        public void Add(string? ipv4, string? ipv6)
        {
            if (ipv4 != null)
            {
                four2six[ipv4] = ipv6;
            }

            if (ipv6 != null)
            {
                six2four[ipv6] = ipv4;
            }
        }

        /// <summary>
        /// Gets an array of all known IPv4 addresses.
        /// </summary>
        /// <returns>The known IPv4 addresses.</returns>
        public string[] GetIPv4Addresses()
            => four2six.Keys.ToArray();

        /// <summary>
        /// Gets an array of all known IPv6 addresses.
        /// </summary>
        /// <returns>The known IPv6 addresses.</returns>
        public string[] GetIPv6Addresses()
            => six2four.Keys.ToArray();
    }
}
