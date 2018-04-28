using ArxOne.MrAdvice.Advice;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SD.CacheManager.AOP.Aspects
{
    /// <summary>
    /// 缓存AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class CacheAspect : Attribute, IMethodAdvice
    {
        #region # 字段及构造器

        /// <summary>
        /// 缓存时长
        /// </summary>
        private readonly double _expireSpan;

        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="expireSpan">缓存时长（单位：分钟，-1表示不过期）</param>
        public CacheAspect(double expireSpan = 5)
        {
            this._expireSpan = expireSpan;
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
            try
            {
                bool hasCache = this.OnEntry(context);

                if (!hasCache)
                {
                    context.Proceed();
                    this.OnExit(context);
                }
            }
            catch (Exception exception)
            {
                this.OnException(context, exception);
                throw;
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
                CacheMediator.Set(cacheKey, returnValue, DateTime.Now.AddMinutes(this._expireSpan));
                context.ReturnValue = returnValue;
                hasCache = true;
            }

            return hasCache;
        }
        #endregion

        #region # 方法异常事件 —— void OnException(MethodExecutionArgs args...
        /// <summary>
        /// 方法异常事件
        /// </summary>
        /// <param name="context">方法元数据</param>
        /// <param name="exception">异常</param>
        private void OnException(MethodAdviceContext context, Exception exception)
        {
            throw exception;
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

            if (!this._expireSpan.Equals(-1))
            {
                CacheMediator.Set(cacheKey, context.ReturnValue, DateTime.Now.AddMinutes(this._expireSpan));
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

            //01.方法签名部分
            keyBuilder.Append(context.TargetMethod.DeclaringType.FullName);
            keyBuilder.Append(context.TargetMethod.Name);

            //泛型参数
            foreach (Type genericArg in context.TargetMethod.GetGenericArguments())
            {
                keyBuilder.Append(genericArg.FullName);
            }

            //参数
            foreach (ParameterInfo param in context.TargetMethod.GetParameters())
            {
                keyBuilder.Append(param.Name);
                keyBuilder.Append(param.ParameterType.FullName);
            }

            //02.方法参数值部分
            foreach (object argument in context.Arguments)
            {
                keyBuilder.Append(this.GetJson(argument));
            }

            //03.计算MD5
            string key = keyBuilder.ToString();
            string keyMD5 = this.GetMD5(key);

            //构造最终键
            string finalKey = string.Format("{0}.{1}/{2}", context.TargetMethod.DeclaringType.FullName, context.TargetMethod.Name, keyMD5);

            return finalKey;
        }
        #endregion

        #region # 计算字符串MD5值 —— string GetMD5(string text)
        /// <summary>
        /// 计算字符串MD5值
        /// </summary>
        /// <param name="text">待转换的字符串</param>
        /// <returns>MD5值</returns>
        private string GetMD5(string text)
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
            #region # 验证参数

            if (instance == null)
            {
                return string.Empty;
            }

            #endregion

            try
            {
                JsonSerializerSettings settting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
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
