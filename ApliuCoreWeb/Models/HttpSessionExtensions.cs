using Apliu.Standard.Tools;
using Microsoft.AspNetCore.Http;
using System;

namespace ApliuCoreWeb.Models
{
    public static class HttpSessionExtensions
    {
        /// <summary>
        /// 设置Session的值
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetValue(this ISession session, String key, Object value)
        {
            session.Set(key, JsonHelper.SerializeObject(value));
        }

        public static Object GetValue(this ISession session, String key)
        {
            Byte[] objArry = null;
            session.TryGetValue(key, out objArry);
            return JsonHelper.DeserializeObject(objArry);
        }

        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static void Remove(this ISession session, String key)
        {
            session.Remove(key);
        }
    }
}
