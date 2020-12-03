using System;

namespace IPToolkit.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Local IPv4: " + IP.GetLocalIPv4());
            Console.WriteLine("Local IPv4 async: " + IP.GetLocalIPv4Async().Result);
            Console.WriteLine("Public IPv4: " + IP.GetPublicIPv4());
            Console.WriteLine("Public IPv4 async: " + IP.GetPublicIPv4Async().Result);

            Console.WriteLine("Local IPv6: " + IP.GetLocalIPv6());
            Console.WriteLine("Local IPv6 async: " + IP.GetLocalIPv6Async().Result);
            Console.WriteLine("Public IPv6: " + IP.GetPublicIPv6());
            Console.WriteLine("Public IPv6 async: " + IP.GetPublicIPv6Async().Result);
        }
    }
}