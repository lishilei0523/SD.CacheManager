using System;
using System.Net;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using SD.CacheManager.Interface;
using SD.CacheManager.Memcached.Configuration;

// ReSharper disable once CheckNamespace
namespace SD.CacheManager
{
    /// <summary>
    /// Memcached缓存提供者
    /// </summary>
    public class MemcachedProvider : ICacheAdapter
    {
        #region # 字段及构造器

        /// <summary>
        /// Memcached客户端配置
        /// </summary>
        private static readonly MemcachedClientConfiguration _MemcachedClientConfig;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static MemcachedProvider()
        {
            MemcachedClientConfiguration config = new MemcachedClientConfiguration();

            foreach (ServerElement element in MemcachedConfiguration.Setting.MemcachedServers)
            {
                config.Servers.Add(new IPEndPoint(IPAddress.Parse(element.Host), element.Port));
            }

            config.Protocol = MemcachedProtocol.Binary;

            _MemcachedClientConfig = config;
        }


        /// <summary>
        /// Memcached客户端
        /// </summary>
        private readonly MemcachedClient _memcachedClient;

        /// <summary>
        /// 构造器
        /// </summary>
        public MemcachedProvider()
        {
            this._memcachedClient = new MemcachedClient(_MemcachedClientConfig);
        }

        #endregion

        #region # 写入缓存（无过期时间） —— void Set<T>(string key, T value)
        /// <summary>
        /// 写入缓存（无过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Set<T>(string key, T value)
        {
            this._memcachedClient.Store(StoreMode.Add, key, value);
        }
        #endregion

        #region # 写入缓存（有过期时间） —— void Set<T>(string key, T value, DateTime exp)
        /// <summary>
        /// 写入缓存（有过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="exp">过期时间</param>
        public void Set<T>(string key, T value, DateTime exp)
        {
            this._memcachedClient.Store(StoreMode.Add, key, value, exp);
        }
        #endregion

        #region # 读取缓存 —— T Get<T>(string key)
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public T Get<T>(string key)
        {
            return (T)this._memcachedClient.Get(key);
        }
        #endregion

        #region # 移除缓存 —— void Remove(string key)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            this._memcachedClient.Remove(key);
        }
        #endregion

        #region # 清空缓存 —— void Clear()
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            this._memcachedClient.FlushAll();
        }
        #endregion

        #region # 是否存在缓存 —— bool Exists(string key)
        /// <summary>
        /// 是否存在缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        public bool Exists(string key)
        {
            return this._memcachedClient.Get(key) != null;
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this._memcachedClient != null)
            {
                this._memcachedClient.Dispose();
            }
        }
        #endregion
    }
}
