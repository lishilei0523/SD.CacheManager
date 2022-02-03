using Newtonsoft.Json;
using System;

namespace SD.CacheManager.Redis.Toolkits
{
    /// <summary>
    /// JSON工具
    /// </summary>
    internal static class JsonUtil
    {
        #region # JSON字符串反序列化对象 —— static T AsJsonTo<T>(this string json)
        /// <summary>
        /// JSON字符串反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns>给定类型对象</returns>
        public static T AsJsonTo<T>(this string json)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException(nameof(json), "JSON字符串不可为空！");
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

        #region # 对象序列化JSON字符串 —— static string ToJson(this object instance)
        /// <summary>
        /// 对象序列化JSON字符串
        /// </summary>
        /// <param name="instance">对象</param>
        /// <returns>JSON字符串</returns>
        public static string ToJson(this object instance)
        {
            #region # 验证

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
