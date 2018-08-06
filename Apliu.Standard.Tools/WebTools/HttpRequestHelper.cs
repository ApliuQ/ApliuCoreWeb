using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Apliu.Standard.Tools.Web
{
    public class HttpRequestHelper
    {
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="getUrl"></param>
        /// <returns></returns>
        public static string HttpGet(string getUrl)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(getUrl);
                wbRequest.Method = "GET";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
                    {
                        result = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogWeb("Get请求失败，URL：" + getUrl + "，详情：" + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="paramData"></param>
        /// <param name="headerDic"></param>
        /// <returns></returns>
        public static string HttpPost(string postUrl, string paramData, Dictionary<string, string> headerDic = null)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(postUrl);
                wbRequest.Method = "POST";
                wbRequest.ContentType = "application/x-www-form-urlencoded";
                wbRequest.ContentLength = Encoding.UTF8.GetByteCount(paramData);
                if (headerDic != null && headerDic.Count > 0)
                {
                    foreach (var item in headerDic)
                    {
                        wbRequest.Headers.Add(item.Key, item.Value);
                    }
                }
                using (Stream requestStream = wbRequest.GetRequestStream())
                {
                    using (StreamWriter swrite = new StreamWriter(requestStream))
                    {
                        swrite.Write(paramData);
                    }
                }
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sread = new StreamReader(responseStream))
                    {
                        result = sread.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogWeb("Post请求失败，URL：" + postUrl + "，详情：" + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 异步Post提交数据 主要用于微信公众号
        /// </summary>
        /// <param name="reqUrl">请求地址</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="reqData">请求内容</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> HttpPostAsync(String reqUrl, Encoding encoding, String reqData)
        {
            HttpClient http = new HttpClient();
            HttpContent cont = new StringContent(reqData, encoding, "text/html");
            HttpResponseMessage response = await http.PostAsync(reqUrl, cont);
            return response;
        }

        /// <summary>
        /// 异步Get请求数据 主要用于微信公众号
        /// </summary>
        /// <param name="reqUrl"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> HttpGetAsync(String reqUrl)
        {
            HttpClient http = new HttpClient();
            HttpResponseMessage response = await http.GetAsync(reqUrl);
            return response;
        }

        /// <summary>
        /// 返回HTTP响应消息
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static HttpResponseMessage encapResult(string s)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    s,
                    Encoding.UTF8,
                    "text/html"
                )
            };
        }
    }
}