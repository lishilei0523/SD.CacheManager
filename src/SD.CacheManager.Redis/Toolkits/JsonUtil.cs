using System;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"无法将源JSON反序列化为给定类型\"{typeof(T).Name}\"，请检查类型后重试！", exception);
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
                JsonSerializerOptions settting = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                };

                return JsonSerializer.Serialize(instance, settting);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
        #endregion
    }
}
