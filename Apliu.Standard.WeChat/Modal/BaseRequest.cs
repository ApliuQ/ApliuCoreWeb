using Apliu.Standard.WeChat.Helper;
using System;

namespace Apliu.Standard.WeChat.Modal
{
    public class BaseRequest
    {
        /// <summary>
        /// init登录时获取到User中的Uin
        /// </summary>
        public Int64 Uin { get; set; }
        /// <summary>
        /// 初始化时获取到的参数
        /// </summary>
        public string Sid { get; set; }
        /// <summary>
        /// 初始化时获取到的参数
        /// </summary>
        public string Skey { get; set; }
        /// <summary>
        /// 会随机变
        /// </summary>
        public string DeviceID
        {
            get
            {
                return "e" + OtherUtils.GetRandomNumber(15);
            }
        }
    }
}
