using System.Configuration;

namespace SD.CacheManager.Memcached.Configuration
{
    /// <summary>
    /// 服务器节点
    /// </summary>
    public class ServerElement : ConfigurationElement
    {
        #region # 服务器 —— string Host
        /// <summary>
        /// 服务器
        /// </summary>
        [ConfigurationProperty("host", IsRequired = false, IsKey = true)]
        public string Host
        {
            get { return (string)this["host"]; }
            set { this["host"] = value; }
        }
        #endregion

        #region # 端口 —— int Port
        /// <summary>
        /// 端口
        /// </summary>
        [ConfigurationProperty("port", IsRequired = false, IsKey = true)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }
        #endregion
    }
}
