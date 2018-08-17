using Apliu.Standard.ORM;
using System;

namespace ApliuCoreConsole
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
        public String MsgResponse { get; set; }
        public String MsgXmlBody { get; set; }
    }
}
