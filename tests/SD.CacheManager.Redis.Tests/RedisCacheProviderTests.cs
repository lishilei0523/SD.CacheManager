using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace SD.CacheManager.Redis.StackExchange.Tests
{
    /// <summary>
    /// 缓存测试
    /// </summary>
    [TestClass]
    public class RedisCacheProviderTests
    {
        /// <summary>
        /// 初始化测试
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            //EndPoint[] endpoints = RedisManager.Instance.GetEndPoints();
            //foreach (EndPoint endpoint in endpoints)
            //{
            //    IServer server = RedisManager.Instance.GetServer(endpoint);
            //    server.FlushAllDatabases();
            //}
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
            //EndPoint[] endpoints = RedisManager.Instance.GetEndPoints();
            //foreach (EndPoint endpoint in endpoints)
            //{
            //    IServer server = RedisManager.Instance.GetServer(endpoint);
            //    server.FlushAllDatabases();
            //}
        }
    }
}
