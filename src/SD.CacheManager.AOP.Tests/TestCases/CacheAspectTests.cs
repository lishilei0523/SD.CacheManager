﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.CacheManager.AOP.Aspects;
using SD.Common;
using SD.Toolkits;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace SD.CacheManager.AOP.Tests.TestCases
{
    /// <summary>
    /// 缓存切面测试
    /// </summary>
    [TestClass]
    public class CacheAspectTests
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
            RedisSection.Initialize(configuration);
#endif
        }
        #endregion

        #region # 测试缓存切面 —— void TestCacheAspect()
        /// <summary>
        /// 测试缓存切面
        /// </summary>
        [TestMethod]
        public void TestCacheAspect()
        {
            string product1 = GetProduct("苹果");
            Trace.WriteLine(product1);

            string product2 = GetProduct("苹果");
            Trace.WriteLine(product2);

            string product3 = GetProduct("橘子");
            Trace.WriteLine(product3);
        }
        #endregion


        //Private

        #region # 获取产品 —— static string GetProduct(string keywords)
        /// <summary>
        /// 获取产品
        /// </summary>
        [CacheAspect(2)]
        public static string GetProduct(string keywords)
        {
            return keywords;
        }
        #endregion
    }
}
