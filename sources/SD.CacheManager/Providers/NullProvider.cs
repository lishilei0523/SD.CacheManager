using System;
using System.Collections.Generic;
using SD.CacheManager.Interface;

// ReSharper disable once CheckNamespace
namespace SD.CacheManager
{
    /// <summary>
    /// 空缓存提供者
    /// </summary>
    public class NullProvider : ICacheAdapter
    {
        public void Set<T>(string key, T value) { }
        public void Set<T>(string key, T value, DateTime exp) { }
        public T Get<T>(string key) { return default(T); }
        public void Remove(string key) { }
        public void RemoveRange(IEnumerable<string> keys) { }
        public void RemoveRange(string keyPattern) { }
        public void Clear() { }
        public bool Exists(string key) { return false; }
        public IEnumerable<string> GetKeys(string pattern) { return new string[0]; }
        public void Dispose() { }
    }
}
