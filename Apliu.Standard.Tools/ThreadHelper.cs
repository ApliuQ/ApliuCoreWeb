using System;
using System.Threading;

namespace Apliu.Standard.Tools
{
    /// <summary>
    /// 提供线程异步执行时，保存返回结果的封装类
    /// </summary>
    /// <typeparam name="EveryType"></typeparam>
    [Obsolete]
    public class ThreadResult<EveryType>
    {
        private Func<Object, EveryType> TaskAction;
        private Object ParamsObj;
        /// <summary>
        /// 任务执行的结果，默认值 default(EveryType)
        /// </summary>
        public EveryType Result = default(EveryType);
        /// <summary>
        /// 创建 线程异步执行时，保存返回结果的封装类
        /// </summary>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        public ThreadResult(Func<Object, EveryType> taskAction, Object paramsObj)
        {
            this.TaskAction = taskAction;
            this.ParamsObj = paramsObj;
        }
        /// <summary>
        /// 开始执行任务，并将结果保存到Result属性中
        /// </summary>
        public void RunThread()
        {
            Result = TaskAction.Invoke(ParamsObj);
        }
    }

    /// <summary>
    /// 封装线程类 已提供重载方法
    /// </summary>
    [Obsolete]
    public class ThreadHelper
    {
        private Thread thread = null;
        /// <summary>
        /// 创建封装的线程类，提供重载方法
        /// </summary>
        /// <param name="thread"></param>
        public ThreadHelper(Thread thread)
        {
            this.thread = thread;
        }
        /// <summary>
        /// 在继续执行标准的 COM 和 SendMessage 消息泵处理期间，阻止调用线程，直到由该实例表示的线程终止或经过了指定时间为止。
        /// </summary>
        /// <param name="objParam">设置等待线程终止的时间量的 System.TimeSpan。</param>
        /// <returns>如果线程已终止，则为 true；如果 false 参数指定的时间量已过之后还未终止线程，则为 timeout。</returns>
        public Boolean Join(Object objParam)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(0);
            if (objParam is TimeSpan) timeSpan = (TimeSpan)objParam;
            return thread.Join(timeSpan);
        }
    }
}
