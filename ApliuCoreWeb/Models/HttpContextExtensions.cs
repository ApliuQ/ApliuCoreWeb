using Apliu.Standard.Tools;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ApliuCoreWeb.Models
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 获取完整URL
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientIP(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            else if (string.IsNullOrEmpty(ip))
            {
                ip = "0.0.0.0";
            }
            return ip;
        }

        /// <summary>
        /// 往HttpResponse的Body写入文本内容，格式：text/html，编码：utf-8
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <param name="content"></param>
        public static async Task WriteBodyAsync(this HttpResponse httpResponse, String content)
        {
            httpResponse.ContentType = "text/html";
            Byte[] byteContent = System.Text.ASCIIEncoding.UTF8.GetBytes(content);
            await httpResponse.Body.WriteAsync(byteContent, 0, byteContent.Length);
        }

        /// <summary>
        /// 往HttpResponse的Body写入图片流，格式：image/jpeg
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task WriteBodyAsync(this HttpResponse httpResponse, Bitmap content)
        {
            httpResponse.ContentType = "image/jpeg";
            Byte[] byteContent = content.GetBytes();
            await httpResponse.Body.WriteAsync(byteContent, 0, byteContent.Length);
        }
    }
}
