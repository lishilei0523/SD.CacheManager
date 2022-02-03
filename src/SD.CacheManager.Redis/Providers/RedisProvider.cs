using SD.CacheManager.Redis.Toolkits;
using SD.Toolkits.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace SD.CacheManager
{
    /// <summary>
    /// Redis缓存提供者
    /// </summary>
    public class RedisProvider : ICacheAdapter
    {
        #region # 构造器

        /// <summary>
        /// Redis客户端
        /// </summary>
        private readonly IDatabase _redisClient;

        /// <summary>
        /// 构造器
        /// </summary>
        public RedisProvider()
        {
            this._redisClient = RedisManager.GetDatabase();
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
            string json = value.ToJson();
            this._redisClient.StringSet(key, json);
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
            string json = value.ToJson();
            TimeSpan timeSpan = exp - DateTime.Now;

            this._redisClient.StringSet(key, json, timeSpan);
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
            string json = this._redisClient.StringGet(key);
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }

            T instance = json.AsJsonTo<T>();

            return instance;
        }
        #endregion

        #region # 移除缓存 —— void Remove(string key)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            this._redisClient.KeyDelete(key);
        }
        #endregion

        #region # 移除缓存 —— void RemoveRange(IEnumerable<string> keys)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">缓存键集</param>
        public void RemoveRange(IEnumerable<string> keys)
        {
            #region # 验证

            keys = keys?.ToArray() ?? new string[0];
            if (!keys.Any())
            {
                return;
            }

            #endregion

            ICollection<RedisKey> redisKeys = new HashSet<RedisKey>();
            foreach (string key in keys)
            {
                redisKeys.Add(key);
            }

            this._redisClient.KeyDelete(redisKeys.ToArray());
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
            return this._redisClient.KeyExists(key);
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

        }
        #endregion
    }
}
