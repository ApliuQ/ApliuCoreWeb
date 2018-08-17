using Apliu.Standard.Tools;
using Apliu.Standard.Tools.Web;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Xml;

namespace ApliuCoreWeb.Models.WeChat
{
    public class WxMessageHelp
    {
        /// <summary>
        /// 处理微信公众号的消息
        /// </summary>
        /// <param name="reqData">详细报文（明文）</param>
        /// <returns>返回报文（明文）</returns>
        public string MessageHandle(string reqData)
        {
            string responseContent = String.Empty;
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(new System.IO.MemoryStream(WeChatBase.WxEncoding.GetBytes(reqData)));
                XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");//接收方帐号
                XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");//开发者微信号

                XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
                if (MsgType != null)
                {
                    switch (MsgType.InnerText)
                    {
                        case "event":
                            responseContent = EventHandle(xmldoc);//事件处理
                            break;
                        case "text":
                            responseContent = TextHandle(xmldoc);//接受文本消息处理
                            break;
                    }
                }

                XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
                XmlNode Event = xmldoc.SelectSingleNode("/xml/Event");

                XmlDocument respXml = new XmlDocument();
                respXml.Load(new System.IO.MemoryStream(WeChatBase.WxEncoding.GetBytes(responseContent)));
                XmlNode response = respXml.SelectSingleNode("/xml/Content");

                WeChatMsg weChatMsg = new WeChatMsg()
                {
                    Id = Guid.NewGuid().ToString().ToUpper(),
                    FromUserName = FromUserName.InnerText,
                    ToUserName = ToUserName.InnerText,
                    MsgType = MsgType.InnerText,
                    Content = Content?.InnerText,
                    Event = Event?.InnerText,
                    Response = response?.InnerText,
                    RespXmlBody = responseContent,
                    MsgXmlBody = reqData
                };
                DataAccess.Instance.ORM.Insert(weChatMsg);
            }
            catch (Exception ex)
            {
                Logger.WriteLogAsync($"回复微信公众号消息失败,详情:{ex.Message}");
                Logger.WriteLogAsync($"StackTrace:{ex.StackTrace}");
            }
            return responseContent;
        }

        /// <summary>
        /// 处理事件消息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public string EventHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode Event = xmldoc.SelectSingleNode("/xml/Event");
            XmlNode EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            if (Event != null)
            {
                //菜单单击事件
                if (Event.InnerText.Equals("CLICK"))
                {
                    //Helper.GetUserDetail(Helper.IsExistAccess_Token(), FromUserName.InnerText);//获取用户基本信息
                    if (EventKey.InnerText.Equals("12"))
                    {
                        responseContent = string.Format(WxMessageType.Text,
                            FromUserName.InnerText,
                            ToUserName.InnerText,
                            DateTimeHelper.Ticks,
                            "欢迎查看ApliuTools");
                    }
                }
                else if (Event.InnerText.Equals("subscribe"))//关注公众号时推送消息
                {
                    //Helper.GetUserDetail(Helper.IsExistAccess_Token(), FromUserName.InnerText);//获取用户基本信息
                    responseContent = string.Format(WxMessageType.Text,
                        FromUserName.InnerText,
                        ToUserName.InnerText,
                        DateTimeHelper.Ticks,
                        "欢迎关注ApliuTools");
                }
            }
            //Logger.WriteLogAsync("接收事件日志，开发者微信号：" + ToUserName.InnerText + "，用户OpenId：" + FromUserName.InnerText + "，事件内容：" + Event.InnerText + "，EventKey：" + EventKey.InnerText);
            return responseContent;
        }

        /// <summary>
        /// 处理文本消息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <returns></returns>
        public string TextHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");//接收方帐号
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");//开发者微信号
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
            if (Content != null)
            {
                responseContent = string.Format(WxMessageType.Text,
                   FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTimeHelper.Ticks,
                   RespondText(Content.InnerText));
            }
            //Logger.WriteLogAsync("接收文本消息日志，开发者微信号：" + ToUserName.InnerText + "，用户OpenId：" + FromUserName.InnerText + "，消息内容：" + Content.InnerText);
            return responseContent;
        }

        /// <summary>
        /// 针对消息进行回复
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string RespondText(String content)
        {
            String resp = String.Empty;
            DataRow dataTable = DataAccess.Instance.GetDataRow($"select Content from WeChatAutoReply where (IsFull=0 and Match like '%{content}%') or (IsFull=1 and Match = '{content}') order by IsFull desc,CreateTime desc;");

            if (dataTable != null) resp = dataTable["Content"].ToString();
            else resp = "欢迎使用ApliuTools微信公众号，功能主页：" + ConfigurationJson.Domain;

            return resp;
        }

        /// <summary>
        /// 异步发送指定模板的消息给用户
        /// </summary>
        /// <param name="openId">接收者openId</param>
        /// <param name="accessToken">access_token</param>
        /// <param name="tempId">模板ID</param>
        /// <param name="redirectUrl">模板跳转链接</param>
        /// <param name="reqData">模板数据</param>
        /// <returns></returns>
        public static async Task<String> SendTemplateMessageAsync(string openId, string accessToken, string tempId, string redirectUrl, string reqData)
        {
            string requestUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + WxTokenManager.AccessToken;

            string content = @"{0}
		 ""touser"":""{2}"",
         ""template_id"":""{3}"",
		 ""url"":""{4}"",            
		 ""data"":{5}
        {1}";
            content = string.Format(content, new string[] { "{", "}", openId, tempId, redirectUrl, reqData });

            System.Net.Http.HttpResponseMessage response = await HttpRequestHelper.HttpPostAsync(requestUrl, WeChatBase.WxEncoding, content);
            String result = await response.Content.ReadAsStringAsync();
            //JObject jObj = Newtonsoft.Json.JsonConvert.DeserializeObject(result) as JObject;
            return result;
        }
    }

    public class WxMessageType
    {
        /// <summary>
        /// 普通文本消息
        /// </summary>
        public static string Text
        {
            get
            {
                return @"<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[{3}]]></Content></xml>";
            }
        }
    }
}