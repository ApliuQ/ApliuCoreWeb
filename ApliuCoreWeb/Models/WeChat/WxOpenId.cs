using Apliu.Standard.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace ApliuCoreWeb.Models.WeChat
{
    public class WxOpenId
    {
        /// <summary>
        /// 获取OpenId的地址
        /// </summary>
        private static string OpenIdUri
        {
            get
            {
                return "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + WeChatBase.WxAppId + "&secret=" + WeChatBase.WxAppSecret + "&code={0}&grant_type=authorization_code";
            }
        }

        /// <summary>
        /// 获取UnionId的地址 需先获取OpenId
        /// </summary>
        private static string UnionIdUri
        {
            get
            {
                return "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}";
            }
        }

        /// <summary>
        /// 获取OpenID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetOpenId(string code)
        {
            WebClient client = new WebClient();
            Byte[] responseData = client.DownloadData(string.Format(OpenIdUri, code));
            String content = WeChatBase.WxEncoding.GetString(responseData);
            try
            {
                JObject jObj = Newtonsoft.Json.JsonConvert.DeserializeObject(content) as JObject;
                return jObj["openid"].ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteLogWeb("获取OpenId失败（Code：" + code + "），详情：" + ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// 获取UnionId
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static string GetUnionId(string openid)
        {
            WebClient client = new WebClient();
            Byte[] responseData = client.DownloadData(string.Format(UnionIdUri, WxTokenManager.AccessToken, openid));
            String content = WeChatBase.WxEncoding.GetString(responseData);
            try
            {
                JObject jObj = Newtonsoft.Json.JsonConvert.DeserializeObject(content) as JObject;
                return jObj["unionid"].ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteLogWeb("获取UnionId失败（AccessToken：" + WxTokenManager.AccessToken + "，OpenID：" + openid + "），详情：" + ex.Message);
                return String.Empty;
            }
        }
    }
}