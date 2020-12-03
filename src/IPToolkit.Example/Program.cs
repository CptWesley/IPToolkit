using System;

namespace IPToolkit.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(IP.GetLocalIPv4());
            Console.WriteLine(IP.GetLocalIPv4Async().Result);
            Console.WriteLine(IP.GetPublicIPv4());
            Console.WriteLine(IP.GetPublicIPv4Async().Result);
        }
    }
}