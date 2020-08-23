using SD.CacheManager.AOP.Aspects;
using System;
using System.Diagnostics;

namespace SD.CacheManager.AOP.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            string product1 = GetProduct("苹果");
            Trace.WriteLine(product1);

            string product2 = GetProduct("苹果");
            Trace.WriteLine(product2);

            string product3 = GetProduct("橘子");
            Trace.WriteLine(product3);

            Console.WriteLine("OK");
            Console.ReadKey();
        }

        [CacheAspect(2)]
        public static string GetProduct(string keywords)
        {
            return keywords;
        }
    }
}
