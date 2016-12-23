﻿using SD.CacheManager.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace SD.CacheManager
{
    /// <summary>
    /// 缓存中介者
    /// </summary>
    public static class CacheMediator
    {
        #region # 字段及构造器

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync;

        /// <summary>
        /// 缓存实现类型
        /// </summary>
        private static readonly Type _CacheImplType;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CacheMediator()
        {
            _Sync = new object();

            //读取配置文件获取缓存实现
            Assembly cacheImpAssembly = Assembly.Load(CacheProviderConfiguration.Setting.Assembly);

            _CacheImplType = cacheImpAssembly.GetType(CacheProviderConfiguration.Setting.Type);
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
            using (ICacheAdapter cacheAdapter = (ICacheAdapter)Activator.CreateInstance(_CacheImplType))
            {
                lock (_Sync)
                {
                    cacheAdapter.Set(key, value);
                }
            }
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
            using (ICacheAdapter cacheAdapter = (ICacheAdapter)Activator.CreateInstance(_CacheImplType))
            {
                lock (_Sync)
                {
                    cacheAdapter.Set(key, value, exp);
                }
            }
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
            using (ICacheAdapter cacheAdapter = (ICacheAdapter)Activator.CreateInstance(_CacheImplType))
            {
                return cacheAdapter.Get<T>(key);
            }
        }
        #endregion

        #region # 移除缓存 —— static void Remove(string key)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        {
            using (ICacheAdapter cacheAdapter = (ICacheAdapter)Activator.CreateInstance(_CacheImplType))
            {
                cacheAdapter.Remove(key);
            }
        }
        #endregion

        #region # 移除缓存 —— static void RemoveRange(IEnumerable<string> keys)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">缓存键集</param>
        public static void RemoveRange(IEnumerable<string> keys)
        {
            using (ICacheAdapter cacheAdapter = (ICacheAdapter)Activator.CreateInstance(_CacheImplType))
            {
                cacheAdapter.RemoveRange(keys);
            }
        }
        #endregion

        #region # 移除缓存 —— static void RemoveRange(string keyPattern)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keyPattern">缓存键表达式</param>
        public static void RemoveRange(string keyPattern)
        {
            using (ICacheAdapter cacheAdapter = (ICacheAdapter)Activator.CreateInstance(_CacheImplType))
            {
                cacheAdapter.RemoveRange(keyPattern);
            }
        }
        #endregion

        #region # 清空缓存 —— static void Clear()
        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void Clear()
        {
            using (ICacheAdapter cacheAdapter = (ICacheAdapter)Activator.CreateInstance(_CacheImplType))
            {
                cacheAdapter.Clear();
            }
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
            using (ICacheAdapter cacheAdapter = (ICacheAdapter)Activator.CreateInstance(_CacheImplType))
            {
                lock (_Sync)
                {
                    return cacheAdapter.Exists(key);
                }
            }
        }
        #endregion

        #region # 获取缓存键列表 —— IEnumerable<string> GetKeys(string pattern)
        /// <summary>
        /// 获取缓存键列表
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        /// <returns>缓存键列表</returns>
        public static IEnumerable<string> GetKeys(string pattern)
        {
            using (ICacheAdapter cacheAdapter = (ICacheAdapter)Activator.CreateInstance(_CacheImplType))
            {
                return cacheAdapter.GetKeys(pattern);
            }
        }
        #endregion
    }
}
