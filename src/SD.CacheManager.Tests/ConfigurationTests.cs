using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace SD.CacheManager.Tests
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
        }
    }
}
