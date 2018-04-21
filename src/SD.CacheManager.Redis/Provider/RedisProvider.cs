using System;
using System.Collections.Generic;
using SD.CacheManager.Interface;
using SD.Toolkits.Redis;
using ServiceStack.Redis;

// ReSharper disable once CheckNamespace
namespace SD.CacheManager
{
    /// <summary>
    /// Redis缓存提供者
    /// </summary>
    public class RedisProvider : ICacheAdapter
    {
        #region # 字段及构造器

        /// <summary>
        /// Redis客户端管理器
        /// </summary>
        private readonly IRedisClientsManager _clientsManager;

        /// <summary>
        /// 静态构造器
        /// </summary>
        public RedisProvider()
        {
            this._clientsManager = RedisManager.CreateClientsManager();
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
            using (IRedisClient redisClient = this._clientsManager.GetClient())
            {
                redisClient.Set(key, value);
            }
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
            using (IRedisClient redisClient = this._clientsManager.GetClient())
            {
                redisClient.Set(key, value, exp);
            }
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
            using (IRedisClient redisClient = this._clientsManager.GetReadOnlyClient())
            {
                return redisClient.Get<T>(key);
            }
        }
        #endregion

        #region # 移除缓存 —— void Remove(string key)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            using (IRedisClient redisClient = this._clientsManager.GetClient())
            {
                redisClient.Remove(key);
            }
        }
        #endregion

        #region # 移除缓存 —— void RemoveRange(IEnumerable<string> keys)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">缓存键集</param>
        public void RemoveRange(IEnumerable<string> keys)
        {
            using (IRedisClient redisClient = this._clientsManager.GetClient())
            {
                redisClient.RemoveAll(keys);
            }
        }
        #endregion

        #region # 移除缓存 —— void RemoveRange(string keyPattern)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keyPattern">缓存键表达式</param>
        public void RemoveRange(string keyPattern)
        {
            IEnumerable<string> specKeys = this.GetKeys(keyPattern);

            this.RemoveRange(specKeys);
        }
        #endregion

        #region # 清空缓存 —— void Clear()
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            using (IRedisClient redisClient = this._clientsManager.GetClient())
            {
                redisClient.FlushDb();
            }
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
            using (IRedisClient redisClient = this._clientsManager.GetReadOnlyClient())
            {
                return redisClient.ContainsKey(key);
            }
        }
        #endregion

        #region # 获取缓存键列表 —— IEnumerable<string> GetKeys(string pattern)
        /// <summary>
        /// 获取缓存键列表
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        /// <returns>缓存键列表</returns>
        public IEnumerable<string> GetKeys(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                return new string[0];
            }
            using (IRedisClient redisClient = this._clientsManager.GetReadOnlyClient())
            {
                return redisClient.SearchKeys(pattern);
            }
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this._clientsManager != null)
            {
                this._clientsManager.Dispose();
            }
        }
        #endregion
    }
}
