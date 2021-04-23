using SD.CacheManager.Memcached.Configuration;
using System;
using System.Configuration;

// ReSharper disable once CheckNamespace
namespace SD.CacheManager
{
    /// <summary>
    /// Memcached服务器配置
    /// </summary>
    public class MemcachedSection : ConfigurationSection
    {
        #region # 字段及构造器

        /// <summary>
        /// 单例
        /// </summary>
        private static readonly MemcachedSection _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static MemcachedSection()
        {
            _Setting = (MemcachedSection)ConfigurationManager.GetSection("memcachedConfiguration");

            #region # 非空验证

            if (_Setting == null)
            {
                throw new ApplicationException("Memcached节点未配置，请检查程序！");
            }

            #endregion
        }

        #endregion

        #region # 访问器 —— static MemcachedSection Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static MemcachedSection Setting
        {
            get { return _Setting; }
        }
        #endregion

        #region # 服务器地址列表 —— ServerElementCollection MemcachedServers
        /// <summary>
        /// 服务器地址列表
        /// </summary>
        [ConfigurationProperty("memcachedServers")]
        [ConfigurationCollection(typeof(ServerElementCollection), AddItemName = "server")]
        public ServerElementCollection MemcachedServers
        {
            get
            {
                ServerElementCollection collection = this["memcachedServers"] as ServerElementCollection;
                return collection ?? new ServerElementCollection();
            }
            set { this["memcachedServers"] = value; }
        }
        #endregion
    }
}
