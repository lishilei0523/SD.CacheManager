using ArxOne.MrAdvice.Advice;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SD.CacheManager.AOP.Aspects
{
    /// <summary>
    /// 缓存AOP特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
    public sealed class CacheAspect : Attribute, IMethodAdvice
    {
        #region # 字段及构造器

        /// <summary>
        /// 缓存时长
        /// </summary>
        private readonly TimeSpan? _expiredSpan;

        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="expiredSpan">缓存时长（null表示永不过期）</param>
        public CacheAspect(TimeSpan? expiredSpan = null)
        {
            this._expiredSpan = expiredSpan;
        }

        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="expiredMinutes">缓存时长（单位：分钟）</param>
        public CacheAspect(int expiredMinutes = 5)
        {
            this._expiredSpan = new TimeSpan(0, expiredMinutes, 0);
        }

        #endregion


        //Public

        #region # 拦截方法 —— void Advise(MethodAdviceContext context)
        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="context">方法元数据</param>
        public void Advise(MethodAdviceContext context)
        {
            bool hasCache = this.OnEntry(context);
            if (!hasCache)
            {
                context.Proceed();
                this.OnExit(context);
            }
        }
        #endregion


        //Private

        #region # 方法进入事件 —— bool OnEntry(MethodAdviceContext context)
        /// <summary>
        /// 方法进入事件
        /// </summary>
        /// <param name="context">方法元数据</param>
        private bool OnEntry(MethodAdviceContext context)
        {
            bool hasCache = false;
            string cacheKey = this.BuildCacheKey(context);
            object returnValue = CacheMediator.Get<object>(cacheKey);
            if (returnValue != null)
            {
                if (this._expiredSpan.HasValue)
                {
                    CacheMediator.Set(cacheKey, returnValue, DateTime.Now.Add(this._expiredSpan.Value));
                }
                else
                {
                    CacheMediator.Set(cacheKey, returnValue);
                }

                context.ReturnValue = returnValue;
                hasCache = true;
            }

            return hasCache;
        }
        #endregion

        #region # 方法结束事件 —— void OnExit(MethodAdviceContext context)
        /// <summary>
        /// 方法结束事件
        /// </summary>
        /// <param name="context">方法元数据</param>
        private void OnExit(MethodAdviceContext context)
        {
            if (context.ReturnValue == null)
            {
                return;
            }

            string cacheKey = this.BuildCacheKey(context);

            if (this._expiredSpan.HasValue)
            {
                CacheMediator.Set(cacheKey, context.ReturnValue, DateTime.Now.Add(this._expiredSpan.Value));
            }
            else
            {
                CacheMediator.Set(cacheKey, context.ReturnValue);
            }
        }
        #endregion

        #region # 构造缓存键 —— string BuildCacheKey(MethodAdviceContext context)
        /// <summary>
        /// 构造缓存键
        /// </summary>
        /// <param name="context">方法元数据</param>
        /// <returns>缓存键</returns>
        private string BuildCacheKey(MethodAdviceContext context)
        {
            StringBuilder keyBuilder = new StringBuilder();

            //方法签名
            keyBuilder.Append(context.TargetMethod.DeclaringType?.FullName);
            keyBuilder.Append(context.TargetMethod.Name);

            //类型参数
            foreach (Type typeArgument in context.TargetMethod.GetGenericArguments())
            {
                keyBuilder.Append(typeArgument.FullName);
            }

            //参数
            foreach (ParameterInfo parameter in context.TargetMethod.GetParameters())
            {
                keyBuilder.Append(parameter.Name);
                keyBuilder.Append(parameter.ParameterType.FullName);
            }

            //参数值
            foreach (object argument in context.Arguments)
            {
                string argumentJson = this.GetJson(argument);
                keyBuilder.Append(argumentJson);
            }

            //计算MD5
            string key = keyBuilder.ToString();
            string keyHash = this.GetHash(key);

            //构造最终键
            string finalKey = string.Format("{0}.{1}({2})", context.TargetMethod.DeclaringType?.FullName, context.TargetMethod.Name, keyHash);

            return finalKey;
        }
        #endregion

        #region # 计算字符串MD5值 —— string GetHash(string text)
        /// <summary>
        /// 计算字符串MD5值
        /// </summary>
        /// <param name="text">待转换的字符串</param>
        /// <returns>MD5值</returns>
        private string GetHash(string text)
        {
            byte[] buffer = Encoding.Default.GetBytes(text);
            using (MD5 md5 = MD5.Create())
            {
                buffer = md5.ComputeHash(buffer);
                StringBuilder md5Builder = new StringBuilder();
                foreach (byte @byte in buffer)
                {
                    md5Builder.Append(@byte.ToString("x2"));
                }
                return md5Builder.ToString();
            }
        }
        #endregion

        #region # object序列化JSON字符串 —— string GetJson(object instance)
        /// <summary>
        /// object序列化JSON字符串
        /// </summary>
        /// <param name="instance">object及其子类对象</param>
        /// <returns>JSON字符串</returns>
        private string GetJson(object instance)
        {
            #region # 验证

            if (instance == null)
            {
                return string.Empty;
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
                return string.Empty;
            }
        }
        #endregion
    }
}
