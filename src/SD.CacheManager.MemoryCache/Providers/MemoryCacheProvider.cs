using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

// ReSharper disable once CheckNamespace
namespace SD.CacheManager
{
    /// <summary>
    /// MemoryCache缓存提供者
    /// </summary>
    public class MemoryCacheProvider : ICacheAdapter
    {
        #region # 写入缓存（无过期时间） —— void Set<T>(string key, T value)
        /// <summary>
        /// 写入缓存（无过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Set<T>(string key, T value)
        {
            //如果缓存已存在则清空
            if (MemoryCache.Default.Contains(key))
            {
                MemoryCache.Default.Remove(key);
            }

            CacheItemPolicy policy = new CacheItemPolicy
            {
                Priority = CacheItemPriority.NotRemovable
            };

            MemoryCache.Default.Set(key, value, policy);
        }
        #endregion

        #region # 写入缓存（有过期时间） —— void Set<T>(string key, T value, DateTime expiredTime)
        /// <summary>
        /// 写入缓存（有过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiredTime">过期时间</param>
        public void Set<T>(string key, T value, DateTime expiredTime)
        {
            //如果缓存已存在则清空
            if (MemoryCache.Default.Contains(key))
            {
                MemoryCache.Default.Remove(key);
            }

            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = expiredTime
            };

            MemoryCache.Default.Set(key, value, policy);
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
            T instance = (T)MemoryCache.Default.Get(key);

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
            MemoryCache.Default.Remove(key);
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

            keys = keys?.Distinct().ToArray() ?? new string[0];
            if (!keys.Any())
            {
                return;
            }

            #endregion

            foreach (string key in keys)
            {
                this.Remove(key);
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
            return MemoryCache.Default.Contains(key);
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
