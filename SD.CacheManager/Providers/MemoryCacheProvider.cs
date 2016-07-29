using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using SD.CacheManager.Interface;

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
            if (MemoryCache.Default.Get(key) != null)
            {
                MemoryCache.Default.Remove(key);
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.NotRemovable;

            MemoryCache.Default.Set(key, value, policy);
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
            //如果缓存已存在则清空
            if (MemoryCache.Default.Get(key) != null)
            {
                MemoryCache.Default.Remove(key);
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = exp;

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
            return (T)MemoryCache.Default.Get(key);
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
            foreach (string key in keys)
            {
                this.Remove(key);
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
            foreach (KeyValuePair<string, object> keyValue in MemoryCache.Default.ToArray())
            {
                this.Remove(keyValue.Key);
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

        #region # 获取缓存键列表 —— IEnumerable<string> GetKeys(string pattern)
        /// <summary>
        /// 获取缓存键列表
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        /// <returns>缓存键列表</returns>
        public IEnumerable<string> GetKeys(string pattern)
        {
            IEnumerable<string> allKeys = MemoryCache.Default.ToArray().Select(x => x.Key);

            if (string.IsNullOrWhiteSpace(pattern))
            {
                return new string[0];
            }

            ICollection<string> specKeys = new HashSet<string>();

            foreach (string key in allKeys)
            {
                if (Regex.IsMatch(key, pattern))
                {
                    specKeys.Add(key);
                }
            }

            return specKeys;
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
