using System.Drawing;
using System.IO;

namespace Apliu.Standard.Tools
{
    public class QRCode
    {

        /// <summary>
        /// 生成二维码 返回字节数组
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] CreateCodeSimpleByte(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;

            Bitmap qrCodeImage = CreateCodeSimpleBitmap(content);

            MemoryStream ms = new MemoryStream();
            qrCodeImage?.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            qrCodeImage?.Dispose();
            return ms?.ToArray();
        }

        /// <summary>
        /// 生成二维码 返回图片对象
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
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