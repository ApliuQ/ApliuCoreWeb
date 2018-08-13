using System;

namespace Apliu.Standard.Tools
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 获取系统当前时间戳 Unix
        /// </summary>
        /// <returns></returns>
        public static long getCurrentUnixTime()
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        /// <summary>
        /// 获取指定时间戳 Unix
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long getUnixTime(DateTime dt)
        {
            return (long)(dt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        /// <summary>
        /// 获取服务器当前时间
        /// </summary>
        /// <returns></returns>
        /// 
        public static DateTime DataTimeNow
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 获取当前时间的刻度
        /// </summary>
        public static long Ticks = DataTimeNow.Ticks;
    }
}
