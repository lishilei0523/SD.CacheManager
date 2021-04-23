using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.CacheManager.Memcached.Configuration;
using System.Diagnostics;

namespace SD.CacheManager.Memcached.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void TestConfigurations()
        {
            string cacheProviderType = CacheManagerSection.Setting.CacheProvider.Type;
            string cacheProviderAssembly = CacheManagerSection.Setting.CacheProvider.Assembly;

            Trace.WriteLine(cacheProviderType);
            Trace.WriteLine(cacheProviderAssembly);

            foreach (ServerElement server in MemcachedSection.Setting.MemcachedServers)
            {
                Trace.WriteLine(server.Host);
                Trace.WriteLine(server.Port);
            }
        }
    }
}
