using System;
using SD.CacheManager.AOP.Aspects;

namespace SD.CacheManager.AOPTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //string product1 = GetProduct("123");
            //string product2 = GetProduct("123");
            //string product3 = GetProduct("124");
            Tase();

            Console.WriteLine("OK");
            Console.ReadKey();
        }

        [CacheAspect(2)]
        public static string GetProduct(string keywords)
        {
            return "Hello World";
        }

        [CacheAspect(2)]
        public static string GetProduct(string keywords, string productStatus)
        {
            return "Hello World";
        }

        [CacheAspect(0)]
        public static void Tase()
        {
            Console.WriteLine("ok");
        }
    }
}
