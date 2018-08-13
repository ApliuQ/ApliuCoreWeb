using Apliu.Standard.Tools.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace Apliu.Standard.Tools
{
    public interface ISMSMessage
    {
        ///api/toolapi/sendsms?mobile=18779182730&smscontent=您正在使用短信服务，短信验证码是ACBXDFF，2分钟之内有效，如非本人操作，请忽略本短信。&smsappid=1400075540&smsappkey=b0a0f4466492c96fcd3d1d334cc01749
        bool SendSMS(string Mobile, string SMSContent, out string SendMsg, out string SendLogSql, params string[] Args);
    }

    public class TencentSMS : ISMSMessage
    {
        /// <summary>
        /// 腾讯云 发送短信
        /// </summary>
        /// <param name="Mobile"></param>
        /// <param name="SMSContent"></param>
        /// <param name="Args">SMSAppId,SMSAppKey</param>
        /// <returns></returns>
        public bool SendSMS(string Mobile, string SMSContent, out string SendMsg, out string SendLogSql, params string[] Args)
        {
            if (Args.Length != 2)
            {
                SendLogSql = String.Empty;
                SendMsg = "Apliu：腾讯云短信，参数有误";
                return false;
            }
            return SendSMSParams(Mobile, SMSContent, out SendMsg, out SendLogSql, Args[0], Args[1]);
        }

        /// <summary>
        /// 腾讯云 发送短信
        /// </summary>
        /// <param name="Mobile"></param>
        /// <param name="SMSContent"></param>
        /// <param name="SMSAppId">短信应用API ID</param>
        /// <param name="SMSAppKey">API ID秘钥</param>
        /// <returns></returns>
        public static bool SendSMSParams(string Mobile, string SMSContent, out string SendMsg, out string SendLogSql, string SMSAppId, string SMSAppKey)
        {
            string Rand = new Random().Next(int.MaxValue).ToString().PadLeft(10, '0');
            string sendjson = GetSendJson(Mobile, SMSContent, SMSAppKey, Rand);
            string sendurl = string.Format(SendUrl, SMSAppId, Rand);
            string response = HttpRequestHelper.HttpPost(sendurl, sendjson);
            bool result = AnalysisResponse(response, out SendMsg);
            SendLogSql = string.Format(sqlInsertAll, Guid.NewGuid(), Mobile, SMSContent, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Apliu", "验证码", result, SendMsg);
            Logger.WriteLogWeb("  sendurl:" + sendurl + "  sendjson:" + sendjson + "  response:" + response + "  insertlog:" + SendLogSql);
            //DataAccess.PostData(insertlog);
            return result;
        }

        /// <summary>
        /// 短信接口发送地址
        /// </summary>
        private readonly static string SendUrl = "https://yun.tim.qq.com/v5/tlssmssvr/sendsms?sdkappid={0}&random={1}";
        /// <summary>
        /// 请求Json格式
        /// </summary>
        private readonly static string RequestJson = @"{{
                    ""ext"": ""{0}"",
                    ""extend"": ""{1}"",
                    ""msg"": ""{2}"",
                    ""sig"": ""{3}"",
                    ""tel"": {{
                        ""mobile"": ""{4}"",
                        ""nationcode"": ""{5}""
                    }},
                    ""time"": {6},
                    ""type"": {7}
                }}";

        private static readonly string sqlInsertAll = @"insert into ApSMSHistory(SMSID,MobileNumber,SMSContent,SendTime,CreateUser,SMSType,Project,SendResult,SendMsg) 
                        values('{0}','{1}','{2}','{3}','{4}','{5}','ApliuWeb','{6}','{7}');";

        /// <summary>
        /// 获取发送短信的Json
        /// </summary>
        /// <param name="ext">用户的 session 内容，腾讯 server 回包中会原样返回，可选字段，不需要就填空</param>
        /// <param name="extend">短信码号扩展号，格式为纯数字串，其他格式无效，默认没有开通</param>
        /// <param name="msg">短信消息，utf8 编码，需要匹配审核通过的模板内容</param>
        /// <param name="mobile">电话号码</param>
        /// <param name="nationcode">国家码 中国86</param>
        /// <param name="time">请求发起时间，unix 时间戳（单位：秒），如果和系统时间相差超过 10 分钟则会返回失败</param>
        /// <param name="type">短信类型，Enum{0: 普通短信, 1: 营销短信}（注意：要按需填值，不然会影响到业务的正常使用）</param>
        ///<param name="random">随机数 10位，与发送URL的random一致</param>
        /// <returns></returns>
        private static string GetSendJson(string ext, string extend, string msg, string mobile, string nationcode, long time, string SMSAppKey, string type, string random)
        {
            //"sig" 字段根据公式 sha256（appkey=$appkey&random=$random&time=$time&mobile=$mobile）生成
            string sig = string.Format(@"appkey={0}&random={1}&time={2}&mobile={3}", SMSAppKey, random, time, mobile);
            sig = SecurityHelper.SHA256Encrypt(sig, Encoding.UTF8);
            string json = string.Format(RequestJson, ext, extend, msg, sig, mobile, nationcode, time, type);
            return json;
        }

        /// <summary>
        /// 获取发送短信的Json
        /// </summary>
        /// <param name="Mobile">手机号码</param>
        /// <param name="Message">短信内容</param>
        /// <param name="random">随机数 10位，与发送URL的random一致</param>
        /// <returns></returns>
        private static string GetSendJson(string Mobile, string Message, string SMSAppKey, string Random)
        {
            string json = GetSendJson("", "", Message, Mobile, "86", DateTimeHelper.getCurrentUnixTime(), SMSAppKey, "0", Random);
            return json;
        }

        /// <summary>
        /// 解析返回Json报文
        /// </summary>
        /// <param name="json"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        private static bool AnalysisResponse(string json, out string errorMsg)
        {
            /*@"{
                    ""result"": 0,
                    ""errmsg"": ""OK"",
                    ""ext"": """",
                    ""fee"": 1,
                    ""sid"": ""xxxxxxx""
                }";*/
            if (string.IsNullOrEmpty(json))
            {
                errorMsg = "Apliu：解析Json不能为空";
                return false;
            }

            bool result = false;
            try
            {
                if (JsonConvert.DeserializeObject(json) is JObject jobj)
                {
                    result = jobj["result"].ToString() == "0";
                    errorMsg = jobj["errmsg"].ToString();
                }
                else errorMsg = "Apliu：返回报文Json解析失败，Json：" + json;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return result;
        }
    }
}

