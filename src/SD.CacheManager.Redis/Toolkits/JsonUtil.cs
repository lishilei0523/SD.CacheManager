using Newtonsoft.Json;
using System;

namespace SD.CacheManager.Redis.Toolkits
{
    /// <summary>
    /// JSON工具
    /// </summary>
    internal static class JsonUtil
    {
        #region # JSON字符串反序列化为对象扩展方法 —— static T JsonToObject<T>(this string json)
        /// <summary>
        /// JSON字符串反序列化为对象扩展方法
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns>给定类型对象</returns>
        /// <exception cref="ArgumentNullException">JSON字符串为空</exception>
        /// <exception cref="InvalidOperationException">反序列化为给定类型失败</exception>
        public static T JsonToObject<T>(this string json)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("json", @"JSON字符串不可为空！");
            }

            #endregion

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException(string.Format("无法将源JSON反序列化为给定类型\"{0}\"，请检查类型后重试！", typeof(T).Name));
            }
        }
        #endregion

        #region # object序列化JSON字符串扩展方法 —— static string ToJson(this object instance...
        /// <summary>
        /// object序列化JSON字符串扩展方法
        /// </summary>
        /// <param name="instance">object及其子类对象</param>
        /// <param name="dateFormatString">时间格式字符串</param>
        /// <returns>JSON字符串</returns>
        public static string ToJson(this object instance, string dateFormatString = null)
        {
            #region # 验证参数

            if (instance == null)
            {
                return null;
            }

            #endregion

            try
            {
                JsonSerializerSettings settting = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                if (!string.IsNullOrWhiteSpace(dateFormatString))
                {
                    settting.DateFormatString = dateFormatString;
                }

                return JsonConvert.SerializeObject(instance, Formatting.None, settting);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
        #endregion
    }
}
