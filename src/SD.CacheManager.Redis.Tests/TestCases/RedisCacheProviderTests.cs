using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common;
using SD.Toolkits;
using SD.Toolkits.Redis;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SD.CacheManager.Redis.Tests.TestCases
{
    /// <summary>
    /// 缓存测试
    /// </summary>
    [TestClass]
    public class RedisCacheProviderTests
    {
        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void Init()
        {
#if NETCOREAPP3_1_OR_GREATER
            Assembly entryAssembly = Assembly.GetExecutingAssembly();
            Configuration configuration = ConfigurationExtension.GetConfigurationFromAssembly(entryAssembly);
            CacheManagerSection.Initialize(configuration);
            RedisSection.Initialize(configuration);
#endif
            EndPoint[] endpoints = RedisManager.Instance.GetEndPoints();
            foreach (EndPoint endpoint in endpoints)
            {
                IServer server = RedisManager.Instance.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }

        /// <summary>
        /// 测试插入与读取缓存
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

        /// <summary>
        /// 测试插入与读取缓存
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

        /// <summary>
        /// 测试移除缓存
        /// </summary>
        [TestMethod]
        public void TestRemoveCache()
        {
            string cacheKey = "key";
            string cacheValue = "value";

            CacheMediator.Set(cacheKey, cacheValue);

            //移除
            CacheMediator.Remove(cacheKey);

            string value = CacheMediator.Get<string>(cacheKey);
            Assert.IsNull(value);
        }

        /// <summary>
        /// 测试移除缓存
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

            //移除
            CacheMediator.RemoveRange(new[] { cacheKey1, cacheKey2 });

            Assert.IsFalse(CacheMediator.Exists(cacheKey1));
            Assert.IsFalse(CacheMediator.Exists(cacheKey2));
        }

        /// <summary>
        /// 清理
        /// </summary>
        [TestCleanup]
        public void TestFinalize()
        {
            EndPoint[] endpoints = RedisManager.Instance.GetEndPoints();
            foreach (EndPoint endpoint in endpoints)
            {
                IServer server = RedisManager.Instance.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }
    }
}
