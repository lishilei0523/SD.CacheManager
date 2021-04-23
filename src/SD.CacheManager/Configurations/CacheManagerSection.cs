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
        private static readonly CacheManagerSection _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CacheManagerSection()
        {
            _Setting = (CacheManagerSection)ConfigurationManager.GetSection("sd.cacheManager");

            #region # 非空验证

            if (_Setting == null)
            {
                throw new ApplicationException("SD.CacheManager节点未配置，请检查程序！");
            }

            #endregion
        }

        #endregion

        #region # 访问器 —— static CacheManagerSection Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static CacheManagerSection Setting
        {
            get { return _Setting; }
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
