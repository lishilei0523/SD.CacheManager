using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SD.CacheManager.MemoryCache.Tests.TestCases
{
    /// <summary>
    /// 缓存测试
    /// </summary>
    [TestClass]
    public class MemoryCacheProviderTests
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
            foreach (KeyValuePair<string, object> kv in System.Runtime.Caching.MemoryCache.Default.ToArray())
            {
                System.Runtime.Caching.MemoryCache.Default.Remove(kv.Key);
            }
        }
        #endregion

        #region # 测试清理 —— void Cleanup()
        /// <summary>
        /// 测试清理
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            foreach (KeyValuePair<string, object> kv in System.Runtime.Caching.MemoryCache.Default.ToArray())
            {
                System.Runtime.Caching.MemoryCache.Default.Remove(kv.Key);
            }
        }
        #endregion

        #region # 测试设置与读取缓存 —— void TestSetAndGetCache()
        /// <summary>
        /// 测试设置与读取缓存
        /// </summary>
        [TestMethod]
        public void TestSetAndGetCache()
        {
            for (int index = 0; index < 1000; index++)
            {
                string cacheKey = Guid.NewGuid().ToString();
                string cacheValue = Guid.NewGuid().ToString();

                CacheMediator.Set(cacheKey, cacheValue);

                string value = CacheMediator.Get<string>(cacheKey);
                Assert.IsTrue(value == cacheValue);
            }
        }
        #endregion

        #region # 测试设置与读取缓存 —— void TestSetAndGetCacheParallel()
        /// <summary>
        /// 测试设置与读取缓存
        /// </summary>
        [TestMethod]
        public void TestSetAndGetCacheParallel()
        {
            Parallel.For(0, 1000, index =>
            {
                string cacheKey = Guid.NewGuid().ToString();
                string cacheValue = Guid.NewGuid().ToString();

                CacheMediator.Set(cacheKey, cacheValue);

                string value = CacheMediator.Get<string>(cacheKey);
                Assert.IsTrue(value == cacheValue);
            });
        }
        #endregion

        #region # 测试删除缓存 —— void TestRemoveCache()
        /// <summary>
        /// 测试删除缓存
        /// </summary>
        [TestMethod]
        public void TestRemoveCache()
        {
            string cacheKey = "key";
            string cacheValue = "value";

            CacheMediator.Set(cacheKey, cacheValue);

            //删除
            CacheMediator.Remove(cacheKey);

            string value = CacheMediator.Get<string>(cacheKey);
            Assert.IsNull(value);
        }
        #endregion

        #region # 测试删除缓存 —— void TestRemoveRangeCache_Keys()
        /// <summary>
        /// 测试删除缓存
        /// </summary>
        [TestMethod]
        public void TestRemoveRangeCache_Keys()
        {
            string cacheKey1 = "key1";
            string cacheKey2 = "key2";
            string cacheValue1 = "value1";
            string cacheValue2 = "value2";

            CacheMediator.Set(cacheKey1, cacheValue1);
            CacheMediator.Set(cacheKey2, cacheValue2);

            //删除
            CacheMediator.RemoveRange(new[] { cacheKey1, cacheKey2 });

            Assert.IsFalse(CacheMediator.Exists(cacheKey1));
            Assert.IsFalse(CacheMediator.Exists(cacheKey2));
        }
        #endregion
    }
}
