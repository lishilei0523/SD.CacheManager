using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace SD.CacheManager.Tests.TestCases
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestInitialize]
        public void Initialize()
        {
#if NETCOREAPP3_1_OR_GREATER
            Assembly entryAssembly = Assembly.GetExecutingAssembly();
            Configuration configuration = ConfigurationExtension.GetConfigurationFromAssembly(entryAssembly);
            CacheManagerSection.Initialize(configuration);
#endif
        }

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
