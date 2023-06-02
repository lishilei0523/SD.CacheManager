using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace SD.CacheManager
{
    /// <summary>
    /// 空缓存提供者
    /// </summary>
    public class NullProvider : ICacheAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        public void Set<T>(string key, T value) { }

        /// <summary>
        /// 
        /// </summary>
        public void Set<T>(string key, T value, DateTime expiredTime) { }

        /// <summary>
        /// 
        /// </summary>
        public T Get<T>(string key) { return default; }

        /// <summary>
        /// 
        /// </summary>
        public void Remove(string key) { }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveRange(IEnumerable<string> keys) { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Exists(string key) { return false; }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() { }
    }
}
