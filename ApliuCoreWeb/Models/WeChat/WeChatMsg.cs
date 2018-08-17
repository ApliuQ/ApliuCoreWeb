using Apliu.Standard.ORM;
using Apliu.Standard.Tools;
using System;

namespace ApliuCoreWeb.Models.WeChat
{
    public class WeChatMsg : IModelORM
    {
        [Identity(true)]
        public String Id { get; set; }
        public String FromUserName { get; set; }
        public String ToUserName { get; set; }
        public String MsgType { get; set; }
        public String Event { get; set; }
        public String Content { get; set; }
        public String Response { get; set; }
        public String RespXmlBody { get; set; }
        public String MsgXmlBody { get; set; }
        public String ReceiveTime => DateTimeHelper.DataTimeNow.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
