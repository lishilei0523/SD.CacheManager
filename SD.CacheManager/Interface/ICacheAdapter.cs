using System;
using System.Collections.Generic;

namespace SD.CacheManager.Interface
{
    /// <summary>
    /// 缓存适配器接口
    /// </summary>
    public interface ICacheAdapter : IDisposable
    {
        #region # 写入缓存（无过期时间） —— void Set<T>(string key, T value)
        /// <summary>
        /// 写入缓存（无过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Set<T>(string key, T value);
        #endregion

        #region # 写入缓存（有过期时间） —— void Set<T>(string key, T value, DateTime exp)
        /// <summary>
        /// 写入缓存（有过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="exp">过期时间</param>
        void Set<T>(string key, T value, DateTime exp);
        #endregion

        #region # 读取缓存 —— T Get<T>(string key)
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        T Get<T>(string key);
        #endregion

        #region # 移除缓存 —— void Remove(string key)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);
        #endregion

        #region # 移除缓存 —— void RemoveRange(IEnumerable<string> keys)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">缓存键集</param>
        void RemoveRange(IEnumerable<string> keys);
        #endregion

        #region # 移除缓存 —— void RemoveRange(string keyPattern)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keyPattern">缓存键表达式</param>
        void RemoveRange(string keyPattern);
        #endregion

        #region # 清空缓存 —— void Clear()
        /// <summary>
        /// 清空缓存
        /// </summary>
        void Clear();
        #endregion

        #region # 是否存在缓存 —— bool Exists(string key)
        /// <summary>
        /// 是否存在缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        bool Exists(string key);
        #endregion

        #region # 获取缓存键列表 —— IEnumerable<string> GetKeys(string pattern)
        /// <summary>
        /// 获取缓存键列表
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        /// <returns>缓存键列表</returns>
        IEnumerable<string> GetKeys(string pattern);
        #endregion
    }
}
