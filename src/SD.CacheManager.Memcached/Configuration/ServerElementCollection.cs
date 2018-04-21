using System.Configuration;

namespace SD.CacheManager.Memcached.Configuration
{
    /// <summary>
    /// 服务器节点集合
    /// </summary>
    internal class ServerElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 创建新配置节点
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServerElement();
        }

        /// <summary>
        /// 获取节点键
        /// </summary>
        /// <param name="element">节点</param>
        /// <returns>节点键</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServerElement)element).Host;
        }
    }
}
