using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using PostSharp.Aspects;

namespace SD.CacheManager.AOP.Aspects
{
    /// <summary>
    /// 缓存AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class CacheAspect : OnMethodBoundaryAspect
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

        #region # 方法进入事件 —— void OnEntry(MethodExecutionArgs args)
        /// <summary>
        /// 方法进入事件
        /// </summary>
        /// <param name="args">方法元数据</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            string cacheKey = this.BuildCacheKey(args);

            object returnValue = CacheMediator.Get<object>(cacheKey);

            if (returnValue != null)
            {
                CacheMediator.Set(cacheKey, returnValue, DateTime.Now.AddMinutes(this._expireSpan));
                args.FlowBehavior = FlowBehavior.Return;
                args.ReturnValue = returnValue;
            }
        }
        #endregion

        #region # 方法异常事件 —— void OnException(MethodExecutionArgs args)
        /// <summary>
        /// 方法异常事件
        /// </summary>
        /// <param name="args">方法元数据</param>
        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Default;
            throw args.Exception;
        }
        #endregion

        #region # 方法结束事件 —— void OnExit(MethodExecutionArgs args)
        /// <summary>
        /// 方法结束事件
        /// </summary>
        /// <param name="args"></param>
        public override void OnExit(MethodExecutionArgs args)
        {
            if (args.ReturnValue == null)
            {
                return;
            }

            string cacheKey = this.BuildCacheKey(args);

            if (!this._expireSpan.Equals(-1))
            {
                CacheMediator.Set(cacheKey, args.ReturnValue, DateTime.Now.AddMinutes(this._expireSpan));
            }
            else
            {
                CacheMediator.Set(cacheKey, args.ReturnValue);
            }
        }
        #endregion


        //Private

        #region # 构造缓存键 —— string BuildCacheKey(MethodExecutionArgs args)
        /// <summary>
        /// 构造缓存键
        /// </summary>
        /// <param name="args">方法元数据</param>
        /// <returns>缓存键</returns>
        private string BuildCacheKey(MethodExecutionArgs args)
        {
            StringBuilder keyBuilder = new StringBuilder();

            //01.方法签名部分
            keyBuilder.Append(args.Method.DeclaringType.FullName);
            keyBuilder.Append(args.Method.Name);

            //泛型参数
            foreach (Type genericArg in args.Method.GetGenericArguments())
            {
                keyBuilder.Append(genericArg.FullName);
            }

            //参数
            foreach (ParameterInfo param in args.Method.GetParameters())
            {
                keyBuilder.Append(param.Name);
                keyBuilder.Append(param.ParameterType.FullName);
            }

            //02.方法参数值部分
            foreach (object argument in args.Arguments)
            {
                keyBuilder.Append(this.GetJson(argument));
            }

            //03.计算MD5
            string key = keyBuilder.ToString();
            string keyMD5 = this.GetMD5(key);

            //构造最终键
            string finalKey = string.Format("{0}.{1}/{2}", args.Method.DeclaringType.FullName, args.Method.Name, keyMD5);

            return finalKey;
        }
        #endregion

        #region # 计算字符串MD5值 —— string GetMD5( string str)
        /// <summary>
        /// 计算字符串MD5值
        /// </summary>
        /// <param name="str">待转换的字符串</param>
        /// <returns>MD5值</returns>
        private string GetMD5(string str)
        {
            byte[] buffer = Encoding.Default.GetBytes(str);
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
