using System;
using System.Reflection;
using System.Text;
using PostSharp.Aspects;
using SD.CacheManager.Mediator;

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
            string cacheKey = this.BuildCacheKey(args.Method);

            object returnValue = CacheMediator.Get<object>(cacheKey);

            if (returnValue != null)
            {
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
            string cacheKey = this.BuildCacheKey(args.Method);

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

        #region # 构造缓存键 —— string BuildCacheKey(MethodBase methodBase)
        /// <summary>
        /// 构造缓存键
        /// </summary>
        /// <param name="methodBase">方法元数据</param>
        /// <returns>缓存键</returns>
        private string BuildCacheKey(MethodBase methodBase)
        {
            StringBuilder keyBuilder = new StringBuilder();

            keyBuilder.Append(methodBase.DeclaringType.FullName);
            keyBuilder.Append(methodBase.Name);

            foreach (Type genericArg in methodBase.GetGenericArguments())
            {
                keyBuilder.Append(genericArg.FullName);
            }

            foreach (ParameterInfo param in methodBase.GetParameters())
            {
                keyBuilder.Append(param.Name);
                keyBuilder.Append(param.ParameterType.FullName);
            }

            return keyBuilder.ToString();
        }
        #endregion
    }
}
