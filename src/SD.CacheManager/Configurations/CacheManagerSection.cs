using SD.CacheManager.Configurations;
using System;
using System.Configuration;

// ReSharper disable once CheckNamespace
namespace SD.CacheManager
{
    /// <summary>
    /// SD.CacheManager配置
    /// </summary>
    public class CacheManagerSection : ConfigurationSection
    {
        #region # 字段及构造器

        /// <summary>
        /// 单例
        /// </summary>
        private static CacheManagerSection _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CacheManagerSection()
        {
            _Setting = null;
        }

        #endregion

        #region # 初始化 —— static void Initialize(Configuration configuration)
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configuration">配置</param>
        public static void Initialize(Configuration configuration)
        {
            #region # 验证

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "配置不可为空！");
            }

            #endregion

            _Setting = (CacheManagerSection)configuration.GetSection("sd.cacheManager");
        }
        #endregion

        #region # 访问器 —— static CacheManagerSection Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static CacheManagerSection Setting
        {
            get
            {
                if (_Setting == null)
                {
                    _Setting = (CacheManagerSection)ConfigurationManager.GetSection("sd.cacheManager");
                }
                if (_Setting == null)
                {
                    throw new ApplicationException("SD.CacheManager节点未配置，请检查程序！");
                }

                return _Setting;
            }
        }
        #endregion

        #region # 缓存提供者节点 —— CacheProviderElement CacheProvider
        /// <summary>
        /// 缓存提供者节点
        /// </summary>
        [ConfigurationProperty("cacheProvider", IsRequired = true)]
        public CacheProviderElement CacheProvider
        {
            get { return (CacheProviderElement)this["cacheProvider"]; }
            set { this["cacheProvider"] = value; }
        }
        #endregion
    }
}
