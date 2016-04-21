using System;
using System.Reflection;
using Autofac;
using SD.CacheManager.Configuration;
using SD.CacheManager.Interface;

namespace SD.CacheManager.Mediator
{
    /// <summary>
    /// 缓存中介者
    /// </summary>
    public static class CacheMediator
    {
        #region # 字段及构造器

        /// <summary>
        /// Autofac依赖注入容器
        /// </summary>
        private static readonly IContainer _Container;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CacheMediator()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //读取配置文件获取缓存实现
            Assembly cacheImpAssembly = Assembly.Load(CacheConfiguration.Setting.Assembly);
            Type cacheImplType = cacheImpAssembly.GetType(CacheConfiguration.Setting.Type);

            builder.RegisterType(cacheImplType).As(typeof(ICacheAdapter));

            _Container = builder.Build();
        }

        #endregion

        #region # 写入缓存（无过期时间） —— static void Set<T>(string key, T value)
        /// <summary>
        /// 写入缓存（无过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set<T>(string key, T value)
        {
            ICacheAdapter cacheAdapter = _Container.Resolve<ICacheAdapter>();
            cacheAdapter.Set(key, value);
        }
        #endregion

        #region # 写入缓存（有过期时间） —— static void Set<T>(string key, T value, DateTime exp)
        /// <summary>
        /// 写入缓存（有过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="exp">过期时间</param>
        public static void Set<T>(string key, T value, DateTime exp)
        {
            ICacheAdapter cacheAdapter = _Container.Resolve<ICacheAdapter>();
            cacheAdapter.Set(key, value, exp);
        }
        #endregion

        #region # 读取缓存 —— static T Get<T>(string key)
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static T Get<T>(string key)
        {
            ICacheAdapter cacheAdapter = _Container.Resolve<ICacheAdapter>();
            return cacheAdapter.Get<T>(key);
        }
        #endregion

        #region # 移除缓存 —— static void Remove(string key)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        {
            ICacheAdapter cacheAdapter = _Container.Resolve<ICacheAdapter>();
            cacheAdapter.Remove(key);
        }
        #endregion

        #region # 清空缓存 —— static void Clear()
        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void Clear()
        {
            ICacheAdapter cacheAdapter = _Container.Resolve<ICacheAdapter>();
            cacheAdapter.Clear();
        }
        #endregion

        #region # 是否存在缓存 —— static bool Exists(string key)
        /// <summary>
        /// 是否存在缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        public static bool Exists(string key)
        {
            ICacheAdapter cacheAdapter = _Container.Resolve<ICacheAdapter>();
            return cacheAdapter.Exists(key);
        }
        #endregion
    }
}
