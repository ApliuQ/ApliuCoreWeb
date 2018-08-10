using Apliu.Standard.Tools;
using ApliuCoreWeb.Models;
using ApliuCoreWeb.Models.WeChat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApliuCoreWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WxController : ControllerBase
    {
        /// <summary>
        /// 微信公众号获取Token Api 消息验证、接收微信服务器推送的消息
        /// /api/wx
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [HttpPost]
        public async Task Index()
        {
            String respContent = "Error: 非法请求";
            string signature = HttpContext.Request.Query["signature"];
            string timestamp = HttpContext.Request.Query["timestamp"];
            string nonce = HttpContext.Request.Query["nonce"];
            string echostr = HttpContext.Request.Query["echostr"];
            string openid = HttpContext.Request.Query["openid"];
            //用于验证加密内容的签名
            string msg_signature = HttpContext.Request.Query["msg_signature"];
            string encrypt_type = HttpContext.Request.Query["encrypt_type"];

            Logger.WriteLogAsync("请求方式：" + HttpContext.Request.Method.ToUpper() + "，请求原地址：" + HttpContext.Request.GetAbsoluteUri().ToString());

            //验证消息是否来自微信服务器
            if (WeChatBase.CheckSignature(signature, timestamp, nonce))
            {
                if (HttpContext.Request.Method.ToUpper() == "GET")
                {
                    if (!string.IsNullOrEmpty(echostr))
                    {
                        respContent = echostr;
                    }
                }
                else if (HttpContext.Request.Method.ToUpper() == "POST")
                {
                    respContent = "Error: 处理失败";
                    try
                    {
                        using (Stream stream = Request.Body)
                        {
                            Byte[] postBytes = new Byte[stream.Length];
                            stream.Read(postBytes, 0, (Int32)stream.Length);
                            string beforeReqData = WeChatBase.WxEncoding.GetString(postBytes);
                            if (!string.IsNullOrEmpty(beforeReqData))
                            {
                                if ("aes".Equals(encrypt_type))//(WeChatBase.IsSecurity)
                                {
                                    Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(WeChatBase.WxToken, WeChatBase.WxEncodingAESKey, WeChatBase.WxAppId);
                                    string afterReqData = String.Empty;  //解析之后的明文
                                    int reqRet = wxcpt.DecryptMsg(msg_signature, timestamp, nonce, beforeReqData, ref afterReqData);
                                    if (reqRet == 0)
                                    {
                                        WxMessageHelp wxMsgHelp = new WxMessageHelp();
                                        String retMessage = wxMsgHelp.MessageHandle(afterReqData);
                                        string respData = String.Empty; //xml格式的密文
                                        int resqRet = wxcpt.EncryptMsg(retMessage, timestamp, nonce, ref respData);
                                        if (resqRet == 0)
                                        {
                                            respContent = respData;
                                        }
                                        else
                                        {
                                            Logger.WriteLogAsync("Error：接收微信服务器推送的消息，加密报文失败，ret: " + resqRet);
                                        }
                                    }
                                    else
                                    {
                                        Logger.WriteLogAsync("Error：接收微信服务器推送的消息，解密报文失败，ret: " + reqRet);
                                    }
                                }
                                else
                                {
                                    WxMessageHelp wxMsgHelp = new WxMessageHelp();
                                    String retMessage = wxMsgHelp.MessageHandle(beforeReqData);
                                    respContent = retMessage;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLogAsync("Error：接收微信服务器推送的消息，处理失败，详情: " + ex.Message);
                    }
                }
            }
            await Response.WriteAsync(respContent);
        }

        /// <summary>
        /// 默认是Post
        /// </summary>
        /// <returns></returns>
        [Route("test")]
        [HttpGet]
        public async Task Test()
        {
            String respContent = "ERROR";
            try
            {
                //DataAccess.Instance.TestCeshi();

                #region 测试数据库事务
                //SELECT * FROM sysprocesses where loginame='apliuweb'
                //select * from test;waitfor delay '00:00:05';
                //如果加上(nolock)标记，则是以ReadUncommitted执行该表的查询事务

                /*
                DataAccess.Instance.BeginTransaction(10);

                string sql01 = "insert into Test values('" + Guid.NewGuid().ToString().ToLower() + "','BeginTransaction001','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
                bool p1 = DataAccess.Instance.PostData(sql01);

                string sql02 = "insert into Test values('" + Guid.NewGuid().ToString().ToLower() + "','BeginTransaction002','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
                bool p2 = DataAccess.Instance.PostData(sql02);


                DataSet ds = DataAccess.Instance.GetData("select * from Test where CreateTime like '%" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:s") + "%';waitfor delay '00:00:01';");
                if (ds != null && ds.Tables.Count > 0) respContent = JsonHelper.JsonSerialize(ds.Tables[0]);
                Thread.Sleep(5000);

                DataAccess.Instance.Rollback();
                */
                #endregion
            }
            catch (Exception ex)
            {
                respContent = ex.Message;
            }

            //MsgCryptTest.Sample.Main(null);
            //Logger.WriteLog("Api Wx Test");
            await Response.WriteAsync(respContent);
        }
    }
}
