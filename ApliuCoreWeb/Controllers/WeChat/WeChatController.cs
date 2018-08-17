using ApliuCoreWeb.Models.WeChat;
using Microsoft.AspNetCore.Mvc;
using ApliuCoreWeb.Models;

namespace ApliuCoreWeb.Controllers.WeChat
{
    public class WeChatController : Controller
    {
        public IActionResult JSSDK()
        {
            var jssdk = new WxJSSDKHelper();
            //获取时间戳
            var timestamp = WxJSSDKHelper.GetTimestamp();
            //获取随机码
            var nonceStr = WxJSSDKHelper.GetNoncestr();
            //获取票证
            var jsticket = WxTokenManager.JsApiTicket;
            //获取签名
            var signature = jssdk.GetSignature(jsticket, nonceStr, timestamp, HttpContext.Request.GetAbsoluteUri());

            ViewData["AppId"] = WeChatBase.WxAppId;
            ViewData["Timestamp"] = timestamp;
            ViewData["NonceStr"] = nonceStr;
            ViewData["Signature"] = signature;

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
