using System;
using System.Threading;
using System.Threading.Tasks;

namespace Apliu.Standard.Tools
{
    public class FunctionHelper
    {
        /// <summary>
        /// 以指定的时间 异步 执行有一个参数且有返回值的任务, 时间到达后, 完成或超时则返回结果（任务仍会继续执行）
        /// </summary>
        /// <typeparam name="EveryType">返回结果类型</typeparam>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="callbackAction">执行完成后的回调方法 返回参数为是否执行成功</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <returns></returns>
        public static async Task<EveryType> RunTaskWithTimeoutAsync<EveryType>(Func<Object, EveryType> taskAction, Object paramsObj, TimeSpan timeSpan, Action<Boolean> callbackAction, Boolean throwException)
        {
            Boolean isCompleted = false;
            Task<EveryType> backgroundTask = Task.Factory.StartNew<EveryType>(() => { return RunTaskWithTimeout<EveryType>(taskAction, paramsObj, timeSpan, throwException, out isCompleted); });
            EveryType everyType = await backgroundTask.ConfigureAwait(false);
            callbackAction.Invoke(isCompleted);
            return everyType;
        }

        /// <summary>
        /// 以指定的时间 同步 执行有一个参数且有返回值的任务, 时间到达后, 完成或超时则返回结果（任务仍会继续执行）
        /// </summary>
        /// <typeparam name="EveryType">返回结果类型</typeparam>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <param name="isCompleted">是否执行成功</param>
        /// <returns></returns>
        public static EveryType RunTaskWithTimeout<EveryType>(Func<Object, EveryType> taskAction, Object paramsObj, TimeSpan timeSpan, Boolean throwException, out Boolean isCompleted)
        {
            isCompleted = false;
            EveryType everyType = default(EveryType);
            if (taskAction != null && timeSpan != null && timeSpan.TotalSeconds > 0)
            {
                try
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    CancellationToken cancellationToken = cancellationTokenSource.Token;

                    Task<EveryType> backgroundTask = Task.Factory.StartNew(taskAction, paramsObj, cancellationToken);
                    isCompleted = backgroundTask.Wait(timeSpan);

                    //是否执行完成
                    if (isCompleted)
                    {
                        everyType = backgroundTask.Result;
                    }
                    else
                    {
                        cancellationTokenSource.Cancel();
                        //cancellationToken.ThrowIfCancellationRequested();//仅当取消之后执行该操作，才会backgroundTask.IsCanceled是True

                        if (throwException) throw new Exception(taskAction.Method.ToString() + " 任务执行超时");
                    }
                }
                catch (Exception ex)
                {
                    if (throwException) throw ex;
                }
            }
            return everyType;
        }

        /// <summary>
        /// 以指定的时间 异步 执行任务, 时间到达后, 完成或超时则返回结果（任务仍会继续执行）
        /// </summary>
        /// <typeparam name="EveryType">返回结果类型</typeparam>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="callbackAction">执行完成后的回调方法 返回参数为是否执行成功</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <returns></returns>
        public static async Task RunTaskWithTimeoutAsync(Action taskAction, TimeSpan timeSpan, Action<Boolean> callbackAction, Boolean throwException)
        {
            Task<Boolean> backgroundTask = Task.Factory.StartNew<Boolean>(() => { return RunTaskWithTimeout(taskAction, timeSpan, throwException); });
            Boolean isCompleted = await backgroundTask.ConfigureAwait(false);
            callbackAction.Invoke(isCompleted);
        }

        /// <summary>
        /// 以指定的时间 同步 执行无参且无返回值的任务, 时间到达后, 完成或超时则返回结果（任务仍会继续执行）
        /// </summary>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <returns>是否正常执行完成</returns>
        public static Boolean RunTaskWithTimeout(Action taskAction, TimeSpan timeSpan, Boolean throwException)
        {
            bool isCompleted = false;
            if (taskAction != null && timeSpan != null && timeSpan.TotalSeconds > 0)
            {
                try
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    CancellationToken cancellationToken = cancellationTokenSource.Token;

                    Task backgroundTask = Task.Factory.StartNew(taskAction, cancellationToken);
                    isCompleted = backgroundTask.Wait(timeSpan);

                    //是否执行完成
                    if (!isCompleted)
                    {
                        cancellationTokenSource.Cancel();
                        //cancellationToken.ThrowIfCancellationRequested();//仅当取消之后执行该操作，才会backgroundTask.IsCanceled是True

                        if (throwException) throw new Exception(taskAction.Method.ToString() + " 任务执行超时");
                    }
                }
                catch (Exception ex)
                {
                    if (throwException) throw ex;
                }
            }
            return isCompleted;
        }

        /// <summary>
        /// 以指定的时间 异步 执行有一个参数且有返回值的任务, 时间到达后, 完成或超时则返回结果（任务停止执行）
        /// </summary>
        /// <typeparam name="EveryType">返回结果类型</typeparam>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="callbackAction">执行完成后的回调方法 返回参数为是否执行成功</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <returns></returns>
        public static async Task<EveryType> RunThreadWithTimeoutAsync<EveryType>(Func<Object, EveryType> taskAction, Object paramsObj, TimeSpan timeSpan, Action<Boolean> callbackAction, Boolean throwException)
        {
            Boolean isCompleted = false;
            Task<EveryType> backgroundTask = Task.Factory.StartNew<EveryType>(() => { return FunctionHelper.RunThreadWithTimeout(taskAction, paramsObj, timeSpan, throwException, out isCompleted); });
            EveryType everyType = await backgroundTask.ConfigureAwait(false);
            callbackAction.Invoke(isCompleted);
            return everyType;
        }

        /// <summary>
        /// 以指定的时间 同步 执行有一个参数且有返回值的任务, 时间到达后, 完成或超时则返回结果（任务停止执行）
        /// </summary>
        /// <typeparam name="EveryType">返回结果类型</typeparam>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <param name="isCompleted">是否执行成功</param>
        /// <returns></returns>
        public static EveryType RunThreadWithTimeout<EveryType>(Func<Object, EveryType> taskAction, Object paramsObj, TimeSpan timeSpan, Boolean throwException, out Boolean isCompleted)
        {
            isCompleted = false;
            EveryType everyType = default(EveryType);
            if (taskAction != null && timeSpan != null && timeSpan.TotalSeconds > 0)
            {
                try
                {
                    Thread thread = new Thread(() => { everyType = taskAction.Invoke(paramsObj); });
                    thread.IsBackground = true;
                    thread.Start();
                    isCompleted = thread.Join(timeSpan);
                    thread.Abort();

                    //是否执行完成
                    if (!isCompleted)
                    {
                        if (throwException) throw new Exception(taskAction.Method.ToString() + " 任务执行超时");
                    }
                }
                catch (Exception ex)
                {
                    if (throwException) throw ex;
                }
            }
            return everyType;
        }

        /// <summary>
        /// 以指定的时间 异步 执行任务, 时间到达后, 完成或超时则返回结果（任务停止执行）
        /// </summary>
        /// <typeparam name="EveryType">返回结果类型</typeparam>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="callbackAction">执行完成后的回调方法 返回参数为是否执行成功</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <returns></returns>
        public static async Task RunThreadWithTimeoutAsync(Action taskAction, TimeSpan timeSpan, Action<Boolean> callbackAction, Boolean throwException)
        {
            Task<Boolean> backgroundTask = Task.Factory.StartNew<Boolean>(() => { return FunctionHelper.RunThreadWithTimeout(taskAction, timeSpan, throwException); });
            Boolean isCompleted = await backgroundTask.ConfigureAwait(false);
            callbackAction.Invoke(isCompleted);
        }

        /// <summary>
        /// 以指定的时间 同步 执行无参且无返回值的任务, 时间到达后, 完成或超时则返回结果（任务停止执行）
        /// </summary>
        /// <param name="taskAction">待执行的任务</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="throwException">超时是否抛出异常</param>
        /// <returns>是否正常执行完成</returns>
        public static Boolean RunThreadWithTimeout(Action taskAction, TimeSpan timeSpan, Boolean throwException)
        {
            bool isCompleted = false;
            if (taskAction != null && timeSpan != null && timeSpan.TotalSeconds > 0)
            {
                try
                {
                    Thread thread = new Thread(new ThreadStart(taskAction));
                    thread.IsBackground = true;
                    thread.Start();
                    isCompleted = thread.Join(timeSpan);
                    thread.Abort();
                }
                catch (Exception ex)
                {
                    if (throwException) throw ex;
                }
            }
            return isCompleted;
        }

        /// <summary>
        /// 异步等待指定时间后执行有一个参数且无返回值的任务
        /// </summary>
        /// <param name="action">待执行的任务</param>
        /// <param name="paramsObj">任务的参数</param>
        /// <param name="timeSpan">等待时间</param>
        /// <param name="callbackAction">执行完成后的回调方法</param>
        /// <param name="throwException">是否抛出异常</param>
        /// <returns></returns>
        public static async Task RunTaskTimingAsync(Action<Object> action, Object paramsObj, TimeSpan timeSpan, Action<Boolean> callbackAction, Boolean throwException)
        {
            bool isCompleted = false;
            if (action != null)
            {
                try
                {
                    if (timeSpan != null && timeSpan.TotalSeconds > 0) await Task.Delay(timeSpan);
                    action.Invoke(paramsObj);
                    isCompleted = true;
                }
                catch (Exception ex)
                {
                    if (throwException) throw ex;
                }
            }
            if (callbackAction != null) callbackAction.Invoke(isCompleted);
        }

        /// <summary>
        /// 异步等待指定时间后执行无参且无返回值的任务
        /// </summary>
        /// <param name="action">待执行的任务</param>
        /// <param name="timeSpan">等待时间</param>
        /// <param name="callbackAction">执行完成后的回调方法</param>
        /// <param name="throwException">是否抛出异常</param>
        /// <returns></returns>
        public static async Task RunTaskTimingAsync(Action action, TimeSpan timeSpan, Action<Boolean> callbackAction, Boolean throwException)
        {
            bool isCompleted = false;
            if (action != null)
            {
                try
                {
                    if (timeSpan != null && timeSpan.TotalSeconds > 0) await Task.Delay(timeSpan);
                    action.Invoke();
                    isCompleted = true;
                }
                catch (Exception ex)
                {
                    if (throwException) throw ex;
                }
            }
            if (callbackAction != null) callbackAction.Invoke(isCompleted);
        }

        /// <summary>
        /// 定时重复异步执行指定任务 如需同步执行请再函数后面加上 .wait()，同步等待必须设置endTime
        /// </summary>
        /// <param name="action">待执行的任务</param>
        /// <param name="paramsObj">任务参数</param>
        /// <param name="dueTime">首次执行时间，0s则立刻执行，小于0s则首次不执行等待间隔后再执行</param>
        /// <param name="period">间隔执行时间，必须大于0s</param>
        /// <param name="endTime">结束任务时间，任务运行时间），0s则永不执行，小于0s则永不停止</param>
        /// <param name="throwException">是否抛出异常</param>
        /// <returns></returns>
        public static async Task RunTimerAsync(Action<Object> action, Object paramsObj, TimeSpan dueTime, TimeSpan period, TimeSpan endTime, Boolean throwException)
        {
            if (action != null && period != null && period.TotalSeconds > 0 && endTime.TotalSeconds != 0)
            {
                try
                {
                    if (dueTime == null || dueTime.TotalSeconds < 0) dueTime = period;
                    System.Threading.Timer timer = new System.Threading.Timer((objNull) => { action.Invoke(paramsObj); }, null, dueTime, period);
                    if (endTime.TotalSeconds > 0) await RunTaskTimingAsync(() => { timer.Dispose(); }, endTime, null, throwException);
                }
                catch (Exception ex)
                {
                    if (throwException) throw ex;
                }
            }
        }

        /// <summary>
        /// 定时重复异步执行指定任务 如需同步执行请再函数后面加上 .wait()，同步等待必须设置 endTime
        /// </summary>
        /// <param name="action">待执行的任务</param>
        /// <param name="dueTime">首次执行时间，0s则立刻执行，小于0s则首次不执行等待间隔后再执行</param>
        /// <param name="period">间隔执行时间，必须大于0s</param>
        /// <param name="endTime">结束任务时间，任务运行时间），0s则永不执行，小于0s则永不停止</param>
        /// <param name="throwException">是否抛出异常</param>
        /// <returns></returns>
        public static async Task RunTimerAsync(Action action, TimeSpan dueTime, TimeSpan period, TimeSpan endTime, Boolean throwException)
        {
            if (action != null && period != null && period.TotalSeconds > 0 && endTime.TotalSeconds != 0)
            {
                try
                {
                    if (dueTime == null || dueTime.TotalSeconds < 0) dueTime = period;
                    System.Threading.Timer timer = new System.Threading.Timer((objNull) => { action.Invoke(); }, null, dueTime, period);
                    if (endTime.TotalSeconds > 0) await RunTaskTimingAsync(() => { timer.Dispose(); }, endTime, null, throwException);
                }
                catch (Exception ex)
                {
                    if (throwException) throw ex;
                }
            }
        }
    }
}