﻿using System;
using System.Configuration;
using SD.CacheManager.Interface;
using ServiceStack.Redis;

namespace SD.CacheManager.Redis.Implements
{
    /// <summary>
    /// Redis缓存容器
    /// </summary>
    public class RedisCacheAdapter : ICacheAdapter
    {
        #region # 字段及构造器

        /// <summary>
        /// Redis服务器地址AppSetting键
        /// </summary>
        private const string RedisServerAppSettingKey = "RedisServer";

        /// <summary>
        /// 定义Redis客户端私有字段
        /// </summary>
        private static readonly RedisClient _RedisClient;     // = new RedisClient("127.0.0.1", 6379); 

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RedisCacheAdapter()
        {
            //读取配置文件中的Redis服务端IP地址、端口号
            string ip = ConfigurationManager.AppSettings[RedisServerAppSettingKey];   //127.0.0.1,6379

            //判断是否为空
            if (string.IsNullOrWhiteSpace(ip))
            {
                throw new SystemException("Redis服务端IP地址未配置！");
            }

            string[] redisServer = ip.Split(',');
            //实例化RedisClient
            _RedisClient = new RedisClient(redisServer[0], int.Parse(redisServer[1]));
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
            _RedisClient.Set(key, value);
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
            _RedisClient.Set(key, value, exp);
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
            return _RedisClient.Get<T>(key);
        }
        #endregion

        #region # 移除缓存 —— void Remove(string key)
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            _RedisClient.Remove(key);
        }
        #endregion

        #region # 清空缓存 —— void Clear()
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            _RedisClient.FlushAll();
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
            return _RedisClient.Get<object>(key) != null;
        }
        #endregion
    }
}
