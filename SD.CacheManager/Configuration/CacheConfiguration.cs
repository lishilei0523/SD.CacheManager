using System;
using System.Configuration;

namespace SD.CacheManager.Configuration
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheConfiguration : ConfigurationSection
    {
        #region # 字段及构造器

        /// <summary>
        /// 单例
        /// </summary>
        private static readonly CacheConfiguration _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CacheConfiguration()
        {
            _Setting = (CacheConfiguration)ConfigurationManager.GetSection("cacheConfiguration");

            #region # 非空验证

            if (_Setting == null)
            {
                throw new ApplicationException("缓存节点未配置，请检查程序！");
            }

            #endregion
        }

        #endregion

        #region # 访问器 —— static CacheConfiguration Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static CacheConfiguration Setting
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
