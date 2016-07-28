using System;
using System.Configuration;
using SD.CacheManager.Interface;
using ServiceStack.Redis;

// ReSharper disable once CheckNamespace
namespace SD.CacheManager
{
    /// <summary>
    /// Redis缓存提供者
    /// </summary>
    public class RedisProvider : ICacheAdapter
    {
        #region # 字段及构造器

        /// <summary>
        /// Redis服务器地址AppSetting键
        /// </summary>
        private const string RedisServerAppSettingKey = "RedisServer";

        /// <summary>
        /// Redis服务器地址
        /// </summary>
        private static readonly string[] _RedisServer;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RedisProvider()
        {
            //读取配置文件中的Redis服务端IP地址、端口号
            string ip = ConfigurationManager.AppSettings[RedisServerAppSettingKey];   //127.0.0.1,6379

            //判断是否为空
            if (string.IsNullOrWhiteSpace(ip))
            {
                throw new SystemException("Redis服务端IP地址未配置！");
            }

            _RedisServer = ip.Split(',');

        }


        /// <summary>
        /// Redis客户端
        /// </summary>
        private readonly RedisClient _redisClient;

        /// <summary>
        /// 构造器
        /// </summary>
        public RedisProvider()
        {
            //实例化RedisClient
            this._redisClient = new RedisClient(_RedisServer[0], int.Parse(_RedisServer[1]));
        }

        #endregion

        #region # 写入缓存（无过期时间） —— void Set<T>(string key, T value)
        /// <summary>
        /// 写入缓存（无过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Set<T>(string key, T value)
        {
            this._redisClient.Set(key, value);
        }
        #endregion

        #region # 写入缓存（有过期时间） —— void Set<T>(string key, T value, DateTime exp)
        /// <summary>
        /// 写入缓存（有过期时间）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="exp">过期时间</param>
        public void Set<T>(string key, T value, DateTime exp)
        {
            this._redisClient.Set(key, value, exp);
        }
        #endregion

        #region # 读取缓存 —— T Get<T>(string key)
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public T Get<T>(string key)
        {
            return this._redisClient.Get<T>(key);
        }
        #endregion

        #region # 移除缓存 —— void Remove(string key)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            this._redisClient.Remove(key);
        }
        #endregion

        #region # 清空缓存 —— void Clear()
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            this._redisClient.FlushDb();
        }
        #endregion

        #region # 是否存在缓存 —— bool Exists(string key)
        /// <summary>
        /// 是否存在缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        public bool Exists(string key)
        {
            return this._redisClient.Get<object>(key) != null;
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this._redisClient != null)
            {
                this._redisClient.Dispose();
            }
        }
        #endregion
    }
}
