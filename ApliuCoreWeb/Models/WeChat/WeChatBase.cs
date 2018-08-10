using Apliu.Standard.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ApliuCoreWeb.Models.WeChat
{
    public class WeChatBase
    {
        /// <summary>
        /// 微信服务器绑定域名  格式 https://www.xxxxxx.com
        /// </summary>
        public static String WxDomain
        {
            get
            {
                String wxDomain = ConfigurationJson.Appsettings.WxDomain.Trim();
                if (wxDomain.EndsWith("/")) wxDomain = wxDomain.Substring(0, wxDomain.Length - 1);
                return wxDomain;
            }
        }

        /// <summary>
        /// 开发者ID
        /// </summary>
        public static String WxAppId = ConfigurationJson.Appsettings.WxAppId.Trim();
        /// <summary>
        /// 开发者密码
        /// </summary>
        public static String WxAppSecret = ConfigurationJson.Appsettings.WxAppSecret.Trim();
        /// <summary>
        /// 用户设置的令牌(Token)
        /// </summary>
        public static String WxToken = ConfigurationJson.Appsettings.WxToken.Trim();
        /// <summary>
        /// 消息加解密密钥
        /// </summary>
        public static String WxEncodingAESKey = ConfigurationJson.Appsettings.WxEncodingAESKey.Trim();

        /// <summary>
        /// 微信公众号解析统一编码
        /// </summary>
        public static Encoding WxEncoding = Encoding.UTF8;

        /// <summary>
        /// 微信公众号消息模式是否是安全模式（加密消息），更改设置由公众号配置决定是否加密
        /// </summary>
        [Obsolete]
        public static Boolean IsSecurity = ConfigurationJson.Appsettings.IsSecurity.ToBoolean();

        /// <summary>
        /// 验证消息是否来自微信服务器
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="echostr">随机字符串</param>
        /// <param name="token">公众号基本配置中填写的Token</param>
        /// <returns></returns>
        public static bool CheckSignature(string signature, string timestamp, string nonce)
        {
            if (String.IsNullOrEmpty(signature) || String.IsNullOrEmpty(timestamp) || String.IsNullOrEmpty(nonce))
            {
                return false;
            }

            List<string> sortList = new List<string>
            {
                timestamp,
                nonce,
                WxToken
            };
            sortList.Sort();

            byte[] data = WxEncoding.GetBytes(string.Join("", sortList));
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] result = sha1.ComputeHash(data);//得到哈希值
            string srt = System.BitConverter.ToString(result).Replace("-", "").ToLower(); //转换成为字符串的显示

            if (srt == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 微信公众号JS-SDK使用权限签名算法
        /// </summary>
        /// <param name="nonceStr"></param>
        /// <param name="jsApiTicket"></param>
        /// <param name="timesamp"></param>
        /// <param name="reqUrl"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static int GenarateSinature(string nonceStr, string jsApiTicket, string timesamp, string reqUrl, String signature)
        {
            ArrayList AL = new ArrayList
            {
                nonceStr,
                jsApiTicket,
                timesamp,
                reqUrl
            };
            AL.Sort(new Tencent.WXBizMsgCrypt.DictionarySort());
            string raw = "";
            for (int i = 0; i < AL.Count; ++i)
            {
                raw += AL[i];
            }

            SHA1 sha;
            ASCIIEncoding enc;
            string hash = "";
            try
            {
                sha = new SHA1CryptoServiceProvider();
                enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(raw);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                hash = BitConverter.ToString(dataHashed).Replace("-", "");
                hash = hash.ToLower();
            }
            catch (Exception)
            {
                return 40001;
            }
            signature = hash;
            return 0;
        }
    }
}