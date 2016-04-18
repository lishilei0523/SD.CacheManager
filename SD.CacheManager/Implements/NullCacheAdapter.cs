using System;
using SD.CacheManager.Interface;

namespace SD.CacheManager.Implements
{
    /// <summary>
    /// 空缓存容器
    /// </summary>
    public class NullCacheAdapter : ICacheAdapter
    {
        public void Set<T>(string key, T value) { }
        public void Set<T>(string key, T value, DateTime exp) { }
        public T Get<T>(string key) { return default(T); }
        public void Remove(string key) { }
        public void Clear() { }
    }
}
