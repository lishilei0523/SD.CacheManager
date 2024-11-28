using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace SD.CacheManager.Tests.TestCases
{
    /// <summary>
    /// 配置文件测试
    /// </summary>
    [TestClass]
    public class ConfigurationTests
    {
        #region # 测试初始化 —— void Initialize()
        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
#if NET8_0_OR_GREATER
            Assembly entryAssembly = Assembly.GetExecutingAssembly();
            Configuration configuration = ConfigurationExtension.GetConfigurationFromAssembly(entryAssembly);
            CacheManagerSection.Initialize(configuration);
#endif
        }
        #endregion

        #region # 测试配置文件 —— void TestConfiguration()
        /// <summary>
        /// 测试配置文件
        /// </summary>
        [TestMethod]
        public void TestConfiguration()
        {
            string cacheProviderType = CacheManagerSection.Setting.CacheProvider.Type;
            string cacheProviderAssembly = CacheManagerSection.Setting.CacheProvider.Assembly;

            Trace.WriteLine(cacheProviderType);
            Trace.WriteLine(cacheProviderAssembly);
        }
        #endregion
    }
}
