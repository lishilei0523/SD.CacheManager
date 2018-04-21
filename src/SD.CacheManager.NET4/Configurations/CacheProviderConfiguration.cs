using System;
using System.Configuration;

// ReSharper disable once CheckNamespace
namespace SD.CacheManager
{
    /// <summary>
    /// 缓存提供者配置
    /// </summary>
    internal class CacheProviderConfiguration : ConfigurationSection
    {
        #region # 字段及构造器

        /// <summary>
        /// 单例
        /// </summary>
        private static readonly CacheProviderConfiguration _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CacheProviderConfiguration()
        {
            _Setting = (CacheProviderConfiguration)ConfigurationManager.GetSection("cacheProviderConfiguration");

            #region # 非空验证

            if (_Setting == null)
            {
                throw new ApplicationException("缓存节点未配置，请检查程序！");
            }

            #endregion
        }

        #endregion

        #region # 访问器 —— static CacheProviderConfiguration Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static CacheProviderConfiguration Setting
        {
            get { return _Setting; }
        }
        #endregion

        #region # 类型 —— string Type
        /// <summary>
        /// 类型
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return this["type"].ToString(); }
            set { this["type"] = value; }
        }
        #endregion

        #region # 程序集 —— string Assembly
        /// <summary>
        /// 程序集
        /// </summary>
        [ConfigurationProperty("assembly", IsRequired = true)]
        public string Assembly
        {
            get { return this["assembly"].ToString(); }
            set { this["assembly"] = value; }
        }
        #endregion
    }
}
