using System;
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
        public void Clear() { }
        public bool Exists(string key) { return false; }
    }
}
