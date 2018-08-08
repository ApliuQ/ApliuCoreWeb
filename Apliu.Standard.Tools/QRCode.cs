using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Apliu.Standard.Tools
{
    public class QRCode
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreateCodeSimple(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;

            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ByteArrayContent(CreateCodeSimpleByte(content));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return response;
        }

        public static byte[] CreateCodeSimpleByte(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;

            Bitmap qrCodeImage = CreateCodeSimpleBitmap(content);

            MemoryStream ms = new MemoryStream();
            qrCodeImage?.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            qrCodeImage?.Dispose();
            return ms?.ToArray();
        }

        public static Bitmap CreateCodeSimpleBitmap(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;

            QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            QRCoder.QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCoder.QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, true);
            return qrCodeImage;
        }
    }
}